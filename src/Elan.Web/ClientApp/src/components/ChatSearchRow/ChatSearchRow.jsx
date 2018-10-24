import React, { Component } from 'react';
import './ChatSearchRow.css';

export default class ChatSearchRow extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
        <div className="chat-search-row dropdown-item">
            <div className="user-info">
                <div className="avatar-post">
                        <img src={require("../../assets/default_avatar.jpg")} />
                    </div>
                    <div className="user-post">
                        <strong>{this.props.name}</strong><br/>
                    </div>
                </div>
            </div>
    );
  }
}
