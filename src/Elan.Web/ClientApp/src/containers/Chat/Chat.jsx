import React, { Component } from 'react';
import ChatTopBar from '../../components/ChatTopBar/ChatTopBar';
import ChatMessage from '../../components/ChatMessage/ChatMessage';
import './Chat.css';
import * as signalR from '@aspnet/signalr';
import * as auth from '../../api/AuthService';

export default class Chat extends Component {
    constructor(props) {
        super(props);
        this.state = {
            message: ""
        };
    }
    componentDidMount() {
        let token = Promise.resolve(auth.signIn("test2", "Haslo123."));
        const options = {
            logMessageContent: true,
            logger: signalR.LogLevel.Trace,
            accessTokenFactory: () => token
        };

        // create the connection instance
        this.connection = new signalR.HubConnectionBuilder()
            .withHubProtocol(new signalR.JsonHubProtocol())
            .withUrl("http://localhost:59549/chathub", options)
            .build();

        const startSignalRConnection = connection => connection.start()
            .then(() => console.info('Websocket Connection Established'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.start()
            .then(() => console.info('SignalR Connected'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.on('ReceiveMessage', this.onMessageReceived);
        this.connection.onclose(() => setTimeout(startSignalRConnection(this.connection), 5000));
    }
    onMessageReceived(message) {
        console.log(message);
    }
    sendMessage() {
        this.connection.invoke("SendMessage", "test", "test2", "Test content").catch(err => console.error(err.toString()));;
        this.setState({
            message: ""
        });
    }

    updateInputValue(event) {
        this.setState({
            message: event.target.value
        });
    }
    render() {
        let data = [{ isToMe: true, content: "Message1" },
        { isToMe: false, content: "Message2" },
        { isToMe: true, content: "Message3" },
        { isToMe: false, content: "Message4" },
        { isToMe: true, content: "Message5" }];

        let messages = data.map((msg, index) => <ChatMessage isToMe={msg.isToMe} content={msg.content} key={index} />);

        return (
            <div className="chat-wrapper">
                <div className="chat">
                    <ChatTopBar />
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
