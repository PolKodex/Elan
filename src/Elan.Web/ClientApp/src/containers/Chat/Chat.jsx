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
            chatDisabled: true,
            canLoad: true,
            page: 1,
            totalCount: 0
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
        var listing = {};
        if (message.ToUserId === decodedToken.jti) {
            if (!messages[message.FromUserId]) {
                chatApi.getMessages(message.FromUserId)
                    .then(function(listing) {
                        messages[message.FromUserId] = {
                            totalCount: listing.totalCount,
                            messages: listing.messages
                        };
                        this.setState({ messages });
                    }.bind(this));

            } else {
                messages[message.FromUserId].messages.push({
                    isToMe: true,
                    content: message.Content,
                    sentOn: message.SentOn
                });
                messages[message.FromUserId].totalCount++;
            }

        } else {
            if (!messages[message.ToUserId]) {
                listing = chatApi.getMessages(message.ToUserId);
                messages[message.ToUserId] = {
                    totalCount: listing.totalCount,
                    messages: listing.messages
                };
            } else {
                messages[message.ToUserId].messages.push({
                    isToMe: false,
                    content: message.Content,
                    sentOn: message.SentOn
                });
                messages[message.ToUserId].totalCount++;
            }
        }
        if (this.state.activeUser && message.ToUserId === this.state.activeUser.id) {
            this.setState({ messages }, () => { this.shouldScrollChat = true; });
        } else {
            this.setState({ messages });
        }
    }

    sendMessage() {
        if (!this.state.activeUser) {
            return;
        }

        if (this.state.message.length === 0) {
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
    };


    updateInputValue(event) {
        if (event.target.value.length > 400) {
            event.target.value = event.target.value.substring(0, 400)
        }

        this.setState({
            message: event.target.value
        });
    }

    onUserChange = (user) => {
        const messages = this.state.messages;
        if (!messages[user.id]) {
            chatApi.getMessages(user.id)
                .then(function (data) {
                    messages[user.id] = {
                        messages: data.messages,
                        totalCount: data.totalCount
                    };
                    this.setState({
                        messages,
                        visibleMessages: messages[user.id].messages,
                        totalCount: messages[user.id].totalCount,
                        activeUser: user
                    });
                }.bind(this));
        } else {
            this.setState({
                messages,
                visibleMessages: messages[user.id].messages,
                totalCount: messages[user.id].totalCount,
                activeUser: user
            });
        }

        this.setState({ chatDisabled: false });
    };

    loadOlderMessages = () => {
        chatApi.getMessages(this.state.activeUser.id, this.state.page, 10)
            .then(function (response) {
                let sortFunc = (a, b) => new Date(a.createdOn) - new Date(b.createdOn);
                response.messages.sort(sortFunc);
                var messages = this.state.messages;
                messages[this.state.activeUser.id].messages =
                    response.messages.concat(messages[this.state.activeUser.id].messages);
                this.setState({
                    messages,
                    visibleMessages: messages[this.state.activeUser.id].messages,
                    commentsCount: response.totalCount,
                    page: this.state.page + 1
                });
            }.bind(this));
    };

    handleKeyPress = (e) => {
        if (e.key === 'Enter') {
            this.sendMessage();
        }
    };

    render() {
        let messages = this.state.visibleMessages.map((msg, index) =>
            <ChatMessage isToMe={msg.isToMe}
                content={msg.content}
                date={msg.sentOn}
                key={index}
            />);

        return (
            <div className="chat-wrapper">
                <div className="chat">
                    <ChatTopBar users={this.state.users} activeUser={this.state.activeUser} activeUserChanged={this.onUserChange} />
                    <div className="messages-wrapper" ref={this.messagesRef}>
                        <div className="chat-messages text-center">
                            {this.state.page * 10 < this.state.totalCount && <button className="btn btn-link" onClick={this.loadOlderMessages} disabled={!this.state.canLoad}>Pokaż więcej</button>}
                            {messages}
                        </div>
                    </div>
                    <div className="chat-bottom">
                        <input type="text" className="form-control" value={this.state.message}
                            onKeyPress={this.handleKeyPress.bind(this)}
                            onChange={this.updateInputValue.bind(this)}
                            disabled={this.state.chatDisabled ? "disabled" : ""}
                        />
                        <button className="btn btn-outline-success" onClick={this.sendMessage.bind(this)} disabled={this.state.chatDisabled || this.state.message.length === 0 ? "disabled" : ""}>Wyślij</button>
                    </div>
                </div>
            </div>
        );
    }
}
