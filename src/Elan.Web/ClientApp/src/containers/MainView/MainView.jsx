import React, { Component } from 'react';
import TopBar from '../../components/TopBar/TopBar';
import Login from '../../components/Login/Login';
import Register from '../../components/Register/Register';
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
            <Route exact path="/login" render={() => <Login /> }/>
            <Route exact path="/register" render={() => <Register />}/>
            <Route path="/app" render={() => <App /> }/>
        </div>
      </Router>
    );
  }
}
