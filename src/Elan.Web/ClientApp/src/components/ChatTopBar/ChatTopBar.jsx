import React, { Component } from 'react';
import ChatSearchRow from '../ChatSearchRow/ChatSearchRow';
import './ChatTopBar.css';


export default class ChatTopBar extends Component {
  constructor(props){
    super(props);
  }

  render() {
    //test
    let names = ["Kamil Kuryś", "Maciej Owerczuk", "Karol Nowicki", "Gabriel Mackiewicz", "Beata Hryniewicka"];

    let searchResult = names.map((name, index) => (<ChatSearchRow name={name} key={index}/>))

    return (
        <div className="chat-top-bar">
         <div className="friends-list">
                <div className="dropdown">
                    <button className="btn btn-light dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <div className="avatar-post">
                            <img src={require("../../assets/default_avatar.jpg")} />
                        </div>
                        <div className="user-post">
                            <strong>Marcin Korwek</strong><br/>
                        </div>
                    </button>
                    <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                        <input type="text" placeholder="Szukaj znajomych" className="form-control" />
                        {searchResult}
                    </div>
                </div>
            </div>
         </div>
    );
  }
}
