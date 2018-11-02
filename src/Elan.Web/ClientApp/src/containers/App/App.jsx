import React, { Component } from 'react';
import TopBar from '../../components/TopBar/TopBar';
import Profile from '../../components/Profile/Profile';
import Chat from '../Chat/Chat';
import Wall from '../Wall/Wall';
import { withRouter, Route, Router } from "react-router-dom";
import { createHashHistory } from 'history';
import './App.css';

class App extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
        <div className="app-wrapper">
          <TopBar />
          <div className="app">
            <div className="app-view">
              <div className="page">
                  <div className="page-content">
                    <Route exact path="/app" render={() => <Wall />}/>
                    <Route exact path="/app/account" render={() => <Profile />}/>
                    <Route exact path="/app/groups" render={() => (<p>Groups page</p>)}/>
                    <Route exact path="/app/settings" render={() => (<p>settings page</p>)}/>
                  </div>
              </div>
              <div className="chat-container">
                <Chat />
              </div>
            </div>
          </div>
        </div>
    );
  }
}

export default withRouter(App);
