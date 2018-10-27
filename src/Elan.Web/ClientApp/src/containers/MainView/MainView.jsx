import React, { Component } from 'react';
import TopBar from '../../components/TopBar/TopBar';
import Chat from '../Chat/Chat';
import App from '../App/App';
import { withRouter, Route, Router } from "react-router-dom";
import { createHashHistory } from 'history';
import './MainView.css';

export default class MainView extends Component {
  constructor(props){
    super(props);
    this.history = createHashHistory();
  }

  render() {
    return (
      <Router history={this.history}>
        <div className="main-view">
            <Route exact path="/" render={() => (<p>Login. App in /#/app url</p>)}/>
            <Route exact path="/login" render={() => (<p>Login. App in /#/app url</p>)}/>
            <Route exact path="/register" render={() => (<p>Register</p>)}/>
            <Route path="/app" render={() => <App />}/>
        </div>
      </Router>
    );
  }
}
