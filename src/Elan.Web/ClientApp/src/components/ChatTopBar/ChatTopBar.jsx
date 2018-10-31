import React, { Component } from 'react';
import ChatSearchRow from '../ChatSearchRow/ChatSearchRow';
import './ChatTopBar.css';


export default class ChatTopBar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            filterUsers: props.users
        };
        console.log(props);
        this.users = props.users;
    }


    filterUsers = (value) => {
        let users = this.users.filter((user) => user.name.indexOf(value) !== -1);
        this.setState({ filterUsers: users });
        console.log("CHANGE!");
    }

    onFilterChanged = (e) => {
        this.filterUsers(e.target.value);
    }


    render() {

        let searchResult = this.state.filterUsers.map((user, index) => (<ChatSearchRow user={user} key={index} onSelect={this.props.activeUserChanged} />))

        return (
            <div className="chat-top-bar">
                <div className="friends-list">
                    <div className="dropdown">
                        <button className="btn btn-light dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <div className="avatar-post">
                                <img src={require("../../assets/default_avatar.jpg")} />
                            </div>
                            <div className="user-post">
                                <strong>{this.props.activeUser ? this.props.activeUser.name : 'User not selected'}</strong><br />
                            </div>
                        </button>
                        <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                            <input type="text" placeholder="Szukaj znajomych" className="form-control" onChange={this.onFilterChanged} />
                            {searchResult}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
