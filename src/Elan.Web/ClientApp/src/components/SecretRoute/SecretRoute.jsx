import React, { Component } from 'react';
import { Redirect } from 'react-router';
import * as authService from '../../services/AuthService';
import { Route } from "react-router-dom";


export const SecretRoute = ({ component: Component, ...rest }) => {
    let authenticated = authService.userIsAuthenticated();
    return (
        <Route {...rest} render={(props) =>
            authenticated === true
            ? <Component {...props} />
            : <Redirect to='/login' />}
        />);
};