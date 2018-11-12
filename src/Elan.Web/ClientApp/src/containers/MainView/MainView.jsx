import React, { Component } from 'react';
import Login from '../../components/Login/Login';
import Register from '../../components/Register/Register';
import App from '../App/App';
import { Route, BrowserRouter as Router, Link } from "react-router-dom";
import { createHashHistory } from 'history';
import './MainView.css';

export default class MainView extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
      <Router>
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
