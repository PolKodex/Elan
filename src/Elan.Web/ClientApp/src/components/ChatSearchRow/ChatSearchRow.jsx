import React, { Component } from 'react';
import './ChatSearchRow.css';

export default class ChatSearchRow extends Component {
  render() {
    return (
      <button className="btn btn-light btn-row">
        <div className="chat-search-row dropdown-item" onClick={() => this.props.onSelect(this.props.user)}>
            <div className="user-info">
                <div className="avatar-post">
                        <img src={require("../../assets/default_avatar.jpg")} alt=""/>
                    </div>
                    <div className="user-post">
                        <strong>{this.props.user.name}</strong><br/>
                    </div>
                </div>
            </div>
      </button>
    );
  }
}
