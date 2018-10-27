import React, { Component } from 'react';
import ChatTopBar from '../../components/ChatTopBar/ChatTopBar';
import ChatMessage from '../../components/ChatMessage/ChatMessage';
import './Chat.css';

export default class Chat extends Component {
  constructor(props){
    super(props);
    this.state = {
      activeUser: null,
      messages: []
    }
  }

  onUserChange = (user) => {
    this.setState({activeUser: user});
    //load messages
    //test
    let data = [{isToMe: true, content: "Message1"},
    {isToMe: false, content: "Message2"},
    {isToMe: true, content: "Message3"},
    {isToMe: false, content: "Message4"},
    {isToMe: true, content: "Message5"}];

    this.setState({messages: data});
  }

  render() {
    let users = [{id:1, name:"Kamil Kuryś"}, 
    {id:1, name:"Maciej Owerczuk"}, 
    {id:1, name:"Karol Nowicki"}, 
    {id:1, name:"Gabriel Mackiewicz"}, 
    {id:1, name:"Beata Hryniewicka"}];

    let messages = this.state.messages.map((msg, index) => <ChatMessage isToMe={msg.isToMe} content={msg.content} key={index}/>);

    return (
      <div className="chat-wrapper">
        <div className="chat">
          <ChatTopBar users={users} activeUser={this.state.activeUser} activeUserChanged={this.onUserChange}/>
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
