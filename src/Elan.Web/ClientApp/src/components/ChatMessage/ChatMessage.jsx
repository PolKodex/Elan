import React, { Component } from 'react';
import './ChatMessage.css';

export default class ChatMessage extends Component {
  
  formatDate = (date) => {
  	return `${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()} ${date.getHours()}:${date.getMinutes()}`;
  };

  convertUTCDateToLocalDate = (date) => {
    var newDate = new Date(date.getTime()+date.getTimezoneOffset()*60*1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;   
  }

  render() {
  	let date = new Date(this.props.date);

    if(!this.props.date.includes("Z")) {
      date = this.convertUTCDateToLocalDate(date);
    }

  	date = this.formatDate(date);

    return (
      <div className={this.props.isToMe ? "msg-container-to-me" : "msg-container-from-me"}>
        <div className={this.props.isToMe ? "msg message-to-me" : "msg message-from-me"}>
            <p>{this.props.content}</p>
        </div>
        <p className="msg-date">{date}</p>
      </div>
    );
  }
}
