import React, { Component } from 'react';
import Login from '../../components/Login/Login';
import Register from '../../components/Register/Register';
import App from '../App/App';
import { Route, BrowserRouter as Router } from "react-router-dom";
import { Redirect } from 'react-router';
import * as jwtUtils from '../../utils/JwtUtils';
import './MainView.css';

export default class MainView extends Component {

  render() {
    let authenticated = false;
    const token = localStorage.getItem('token');
    if (token != null){
      const decodedToken = jwtUtils.decodeJwt(localStorage.getItem('token'));
      const currentUnixTimestamp = Math.round((new Date()).getTime() / 1000);

      if (decodedToken.exp > currentUnixTimestamp){
        authenticated = true;
      }
    }

    return (
      <Router>
        <div className="main-view">
            <Route exact path="/" render={() => (<p>Login. App in /#/app url</p>)}/>
            <Route exact path="/login" render={() => <Login /> }/>
            <Route exact path="/register" render={() => <Register />}/>
            <PrivateRoute authed={ authenticated } path="/app" component={ App }/>
        </div>
      </Router>
    );
  }
}

function PrivateRoute ({component: Component, authed, ...rest}) {
  return (
    <Route
      {...rest}
      render={(props) => authed === true
        ? <Component {...props} />
        : <Redirect to={{pathname: '/login', state: {from: props.location}}} />}
    />
  )
}