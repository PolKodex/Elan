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
        let authenticated = authService.userIsAuthenticated();

        debugger;
        return (           
            <Router>
                <div className="main-view">
                    <Route exact path="/login" render={() => <Login />} />
                    <Route exact path="/register" render={() => <Register />} />
                    <SecretRoute auth={ authenticated } path="/" component={App} />}
                </div>
            </Router>
        );
        //                    <PrivateRoute authed={authenticated} path="/" component={App} />           
    }
}
const SecretRoute = ({ auth, component: Component, ...rest }) => (
    <Route {...rest} render={(props) => (
        auth === true
            ? <Component {...props} />
            : <Redirect to='/login' />
    )} />
);