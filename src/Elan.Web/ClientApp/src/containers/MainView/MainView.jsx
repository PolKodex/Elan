import React, { Component } from 'react';
import Login from '../../components/Login/Login';
import Register from '../../components/Register/Register';
import App from '../App/App';
import { Route, BrowserRouter as Router } from "react-router-dom";
import { Redirect } from 'react-router';

import * as authService from '../../services/AuthService';

import './MainView.css';

export default class MainView extends Component {

    render() {
        debugger;
        return (           
            <Router>
                <div className="main-view">
                    <Route exact path="/login" render={() => <Login />} />
                    <Route exact path="/register" render={() => <Register />} />
                    <SecretRoute path="/" component={App} />}
                </div>
            </Router>
        );        
    }
}
const SecretRoute = ({ component: Component, ...rest }) => {
    let authenticated = authService.userIsAuthenticated();
    return (<Route {...rest} render={(props) => (
        authenticated === true
            ? <Component {...props} />
            : <Redirect to='/login' />
    )}/>);
}
