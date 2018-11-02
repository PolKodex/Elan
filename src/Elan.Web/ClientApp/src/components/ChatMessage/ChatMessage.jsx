import React, { Component } from 'react';
import './ChatMessage.css';

export default class ChatMessage extends Component {
  render() {
    return (
      <div className={this.props.isToMe ? "msg-container-to-me" : "msg-container-from-me"}>
        <div className={this.props.isToMe ? "msg message-to-me" : "msg message-from-me"}>
            <p>{this.props.content}</p>
        </div>
      </div>
    );
  }
}
