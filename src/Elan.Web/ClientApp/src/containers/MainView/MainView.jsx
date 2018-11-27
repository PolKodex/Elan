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
        if (token !== null) {
            const decodedToken = jwtUtils.decodeJwt(localStorage.getItem('token'));
            const currentUnixTimestamp = Math.round((new Date()).getTime() / 1000);

            if (decodedToken.exp > currentUnixTimestamp) {
                authenticated = true;
            }
        }
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