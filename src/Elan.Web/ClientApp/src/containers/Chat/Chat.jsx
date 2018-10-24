import React, { Component } from 'react';
import './Chat.css';

export default class Chat extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
      <div className="chat-wrapper">
        <div className="chat">
          Chat
        </div>
      </div>
    );
  }
}
