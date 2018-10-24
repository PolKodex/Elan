import React, { Component } from 'react';
import ChatTopBar from '../../components/ChatTopBar/ChatTopBar';
import ChatMessage from '../../components/ChatMessage/ChatMessage';
import './Chat.css';

export default class Chat extends Component {
  constructor(props){
    super(props);
  }

  render() {
    //test
    let data = [{isToMe: true, content: "Message1"},
    {isToMe: false, content: "Message2"},
    {isToMe: true, content: "Message3"},
    {isToMe: false, content: "Message4"},
    {isToMe: true, content: "Message5"}];

    let messages = data.map((msg, index) => <ChatMessage isToMe={msg.isToMe} content={msg.content} key={index}/>);

    return (
      <div className="chat-wrapper">
        <div className="chat">
          <ChatTopBar />
          <div className="chat-messages">
            {messages}
          </div>
          <div className="chat-bottom">
            <input type="text" className="form-control" />
            <button className="btn btn-outline-success">Wyślij</button>
          </div>
        </div>
      </div>
    );
  }
}
