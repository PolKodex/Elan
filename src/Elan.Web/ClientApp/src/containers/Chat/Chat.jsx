import React, { Component } from 'react';
import ChatTopBar from '../../components/ChatTopBar/ChatTopBar';
import ChatMessage from '../../components/ChatMessage/ChatMessage';
import './Chat.css';

import * as jwtUtils from '../../utils/JwtUtils';

import * as signalR from '@aspnet/signalr';

import * as chatApi from '../../api/ChatApi';
import * as friendsApi from '../../api/FriendsApi';



export default class Chat extends Component {
    constructor(props) {
        super(props);
        this.state = {
            activeUser: null,
            messages: {},
            visibleMessages: [],
            message: "",
            users: [],
            chatDisabled: true
        };

        this.shouldScrollChat = false;
        this.messagesRef = React.createRef();
        const decodedToken = jwtUtils.decodeJwt(localStorage.getItem('token'));
        friendsApi
            .getFriends(decodedToken.jti)
            .then(function (response) {
                var users = this.state.users;

                response.data.map(f => {
                    return {
                        id: f.id,
                        name: f.userName,
                        avatar: f.avatarBase64
                    };
                }).forEach(u => users.push(u));

                this.setState({ users });
            }.bind(this));
    }
    componentDidMount() {
        const options = {
            logMessageContent: true,
            logger: signalR.LogLevel.Trace,
            accessTokenFactory: () => localStorage.getItem('token')
        };

        this.connection = new signalR.HubConnectionBuilder()
            .withHubProtocol(new signalR.JsonHubProtocol())
            .withUrl("/chathub", options)
            .build();

        const startSignalRConnection = connection => connection.start()
            .then(() => console.info('Websocket Connection Established'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.start()
            .then(() => console.info('SignalR Connected'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.on('ReceiveMessage', this.onMessageReceived.bind(this));
        this.connection.onclose(() => setTimeout(startSignalRConnection(this.connection), 5000));
    }

    componentDidUpdate() {
        if (this.shouldScrollChat === true) {
            this.shouldScrollChat = false;
            this.scrollChatToBottom();
        }
    }

    onMessageReceived(message) {
        message = JSON.parse(message);
        var decodedToken = jwtUtils.decodeJwt(localStorage.getItem('token'));
        var messages = this.state.messages;
        if (message.ToUserId === decodedToken.jti) {
            if (!messages[message.FromUserId]) {
                messages[message.FromUserId] = chatApi.getMessages(message.FromUserId).map(m => {
                    return {
                        isToMe: m.IsToMe,
                        content: m.Content,
                        date: m.SentOn
                    };
                });
            } else {
                messages[message.FromUserId].push({
                    isToMe: true,
                    content: message.Content,
                    date: message.SentOn
                });
            }

        } else {
            if (!messages[message.ToUserId]) {
                messages[message.ToUserId] = chatApi.getMessages(message.ToUserId).map(m => {
                    return {
                        isToMe: m.IsToMe,
                        content: m.Content,
                        date: m.SentOn
                    };
                });
            } else {
                messages[message.ToUserId].push({
                    isToMe: false,
                    content: message.Content,
                    date: message.SentOn
                });
            }
        }
        this.setState({ messages }, () => { this.shouldScrollChat = true; });
    }



    sendMessage() {
        if (!this.state.activeUser) {
            return;
        }
        if (this.state.message.length < 1) {
            return;
        }
        this.connection.invoke("SendMessage", this.state.activeUser.id, this.state.message).catch(err => console.error(err.toString()));
        this.setState({
            message: ""
        }, () => { this.shouldScrollChat = true; });

    }

    scrollChatToBottom = () => {
        let sHeight = this.messagesRef.current.scrollHeight;
        let height = this.messagesRef.current.clientHeight;
        let maxScrollTop = sHeight - height;
        this.messagesRef.current.scrollTop = maxScrollTop > 0 ? maxScrollTop : 0;
    }


    updateInputValue(event) {
        this.setState({
            message: event.target.value
        });
    }

    onUserChange = (user) => {
        const messages = this.state.messages;
        if (!messages[user.id]) {
            chatApi.getMessages(user.id)
                .then(function (data) {
                    messages[user.id] = data.map(m => {
                        return {
                            isToMe: m.isToMe,
                            content: m.content,
                            date: m.sentOn
                        };
                    });
                    this.setState({
                        messages,
                        visibleMessages: messages[user.id],
                        activeUser: user
                    });
                }.bind(this));
        } else {
            this.setState({
                messages,
                visibleMessages: messages[user.id],
                activeUser: user
            });
        }

        this.setState({ chatDisabled: false });

    }

    handleKeyPress = (e) => {
        if (e.key === 'Enter') {
            this.sendMessage();
        }
    };

    render() {
        let messages = this.state.visibleMessages.map((msg, index) =>
            <ChatMessage isToMe={msg.isToMe}
                content={msg.content}
                date={msg.date}
                key={index}
            />);

        return (
            <div className="chat-wrapper">
                <div className="chat">
                    <ChatTopBar users={this.state.users} activeUser={this.state.activeUser} activeUserChanged={this.onUserChange} />
                    <div className="messages-wrapper" ref={this.messagesRef}>
                        <div className="chat-messages">
                            {messages}
                        </div>
                    </div>
                    <div className="chat-bottom">
                        <input type="text" className="form-control" value={this.state.message}
                            onKeyPress={this.handleKeyPress.bind(this)}
                            onChange={this.updateInputValue.bind(this)}
                            disabled={this.state.chatDisabled ? "disabled" : ""}
                        />
                        <button className="btn btn-outline-success" onClick={this.sendMessage.bind(this)} disabled={this.state.chatDisabled ? "disabled" : ""}>Wyślij</button>
                    </div>
                </div>
            </div>
        );
    }
}
