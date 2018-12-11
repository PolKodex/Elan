import React, { Component } from 'react';
import Login from '../../components/Login/Login';
import Register from '../../components/Register/Register';
import App from '../App/App';
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import { SecretRoute } from '../../components/SecretRoute/SecretRoute';
import './MainView.css';

export default class MainView extends Component {

    render() {
        return (
            <Router>
                <div className="main-view">
                    <Switch>
                        <Route exact path="/login" render={() => <Login />} />
                        <Route exact path="/register" render={() => <Register />} />
                        <SecretRoute path="/" component={App} />
                    </Switch>
                </div>
            </Router>
        );
    }
}


