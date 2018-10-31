import React, { Component } from 'react';
import ChatTopBar from '../../components/ChatTopBar/ChatTopBar';
import ChatMessage from '../../components/ChatMessage/ChatMessage';
import './Chat.css';

import * as signalR from '@aspnet/signalr';
import * as chatApi from '../../api/ChatApi';

export default class Chat extends Component {
    constructor(props) {
        super(props);
        this.state = {
            activeUser: null,
            messages: {},
            visibleMessages: [],
            message: ""
        };
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

    decodeJwt() {
        var token = localStorage.getItem('token');
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    }

    onMessageReceived(message) {
        message = JSON.parse(message);
        var decodedToken = this.decodeJwt();
        var messages = this.state.messages;

        if (message.ToUserId === decodedToken.jti) {
            if (!messages[message.FromUserId]) {
                messages[message.FromUserId] = chatApi.getMessages(message.FromUserId).map(m => {
                    return {
                        isToMe: m.IsToMe,
                        content: m.Content
                    };
                });
            } else {
                messages[message.FromUserId].push({
                    isToMe: true,
                    content: message.Content
                });
            }

        } else {
            if (!messages[message.ToUserId]) {
                messages[message.ToUserId] = chatApi.getMessages(message.ToUserId).map(m => {
                    return {
                        isToMe: m.IsToMe,
                        content: m.Content
                    };
                });
            } else {
                messages[message.ToUserId].push({
                    isToMe: false,
                    content: message.Content
                });
            }
            this.setState({ messages });
        }
    }

    sendMessage() {
        this.connection.invoke("SendMessage", this.state.activeUser.id, this.state.message).catch(err => console.error(err.toString()));
        this.setState({
            message: ""
        });
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
                            content: m.content
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

    }

    render() {
        let users = [{ id: "fc5c017d-5a2f-4e2c-5cdf-08d63f5e546f", name: "test3" },
        { id: "14246c63-c356-46e8-33fd-08d639229835", name: "test2" },
        { id: "1", name: "Karol Nowicki" },
        { id: "1", name: "Gabriel Mackiewicz" },
        { id: "1", name: "Beata Hryniewicka" }];

        let messages = this.state.visibleMessages.map((msg, index) => <ChatMessage isToMe={msg.isToMe} content={msg.content} key={index} />);

        return (
            <div className="chat-wrapper">
                <div className="chat">
                    <ChatTopBar users={users} activeUser={this.state.activeUser} activeUserChanged={this.onUserChange} />
                    <div className="chat-messages">
                        {messages}
                    </div>
                    <div className="chat-bottom">
                        <input type="text" className="form-control" value={this.state.message} onChange={this.updateInputValue.bind(this)} />
                        <button className="btn btn-outline-success" onClick={this.sendMessage.bind(this)}>Wyślij</button>
                    </div>
                </div>
            </div>
        );
    }
}
