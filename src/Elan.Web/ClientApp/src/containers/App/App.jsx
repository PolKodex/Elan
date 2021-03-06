import React, { Component } from 'react';
import TopBar from '../../components/TopBar/TopBar';
import FooterBar from '../../components/FooterBar/FooterBar';
import Account from '../../components/Account/Account';
import Chat from '../Chat/Chat';
import Wall from '../Wall/Wall';
import Settings from '../../components/Settings/Settings';
import { withRouter, Route } from "react-router-dom";
import './App.css';

class App extends Component {

  render() {
    return (
        <div className="app-wrapper">
          <TopBar />
          <div className="app">
            <div className="app-view">
              <div className="page">
                  <div className="page-content">
                    <Route exact path="/" render={() => <Wall />}/>
                    <Route exact path="/account/:id?" render={(props) => <Account {...props} />}/>
                    <Route exact path="/settings" render={() => <Settings />} />
                  </div>
              </div>
              <div className="chat-container">
                <Chat />
              </div>
            </div>
            </div>
            <FooterBar />
        </div>
    );
  }
}

export default withRouter(App);
