import React, { Component } from 'react';
import * as auth from '../../api/AuthApi';
import { withRouter, Link } from "react-router-dom";
import './Register.css';
import { Redirect } from 'react-router';

export default class Register extends Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            loginMessage: '',
            password: '',
            passwordMessage: '',
            confirmPassword: '',
            confirmPasswordMessage: '',
            email: '',
            emailMessage: '',
            firstName: '',
            lastName: '',
            message: '',
            redirect: false
        }
    }

    loginChange = (event) => {
        this.setState({ login: event.target.value });
    }
    
    passwordChange = (event) => {
        if (event.target.value !== this.state.confirmPassword) {
            this.setState({ confirmPasswordMessage: "Hasła nie są takie same" });
        } else {
            this.setState({ confirmPasswordMessage: "" });
        }

        this.setState({ password: event.target.value });
    }

    confirmPasswordChange = (event) => {
        if (event.target.value !== this.state.password) {
            this.setState({ confirmPasswordMessage: "Hasła nie są takie same" });
        } else {
            this.setState({ confirmPasswordMessage: "" });
        }
        this.setState({ confirmPassword: event.target.value });
    }

    emailChange = (event) => {
        this.setState({ email: event.target.value });
    }

    firstNameChange = (event) => {
        this.setState({ firstName: event.target.value });
    }
    
    lastNameChange = (event) => {
        this.setState({ lastName: event.target.value });
    }

    handleSubmit = () => {
        auth.register(
                this.state.login, 
                this.state.password, 
                this.state.email, 
                this.state.firstName, 
                this.state.lastName)
            .then(function(token) {
                localStorage.setItem('token', token);
                this.setState({ redirect: true });
            }.bind(this))
            .catch((response) => {
                this.handleErrors(response);
            });
    }

    trimError(value) {
        return value.replace(/^\s/, '');
    }

    clearErrors() {
        this.setState({ 
            loginMessage: '',
            passwordMessage: '',
            confirmPasswordMessage: '',
            emailMessage: '',
            message: ''
        });
    }

    displayError(value) {
        value = this.trimError(value);

        if (value.startsWith("User")) {
            this.setState({ 
                loginMessage: this.state.loginMessage += value + '\n'
            });
        }
        else if (value.startsWith("Password")) {
            this.setState({ 
                passwordMessage: this.state.passwordMessage += value + '\n'
            });
        } 
        else if (value.startsWith("Email")) {
            this.setState({ 
                emailMessage: this.state.emailMessage += value + '\n'
            });
        }
        else {
            this.setState({ 
                message: this.state.message += value + '\n'
            });
        }
    }

    handleErrors = (response) => {
        this.clearErrors();
        var parts = response.response.data.split(":");

        if (parts.length === 2) {
            var errors = parts[1].split(",");

            errors.forEach((value) => {
                this.displayError(value);
            });
        }
        else {
            this.setState({ 
                message: response.response.data
            });
        }
    }

    renderRedirect = () => {
        if (this.state.redirect) {
          return <Redirect to='/app' />
        }
      }

    render() {
        return (
            <div className="container login-page">
                { this.renderRedirect() }

                <div className="row login-logo">
                    <div className="">

                    </div>
                </div>

                <div className="row login-main">
                    <div className="offset-md-2 col-md-8 login-form">
                        <div className="card">
                            <div className="card-header">
                                Rejestracja do portalu <strong><span className="brand-color">EL</span>AN</strong>
                            </div>
                            <div className="card-body">
                                <form onSubmit={ this.handleSubmit }>
                                    <div className="form-group">
                                        <label htmlFor="register-login">Login</label>
                                        <input 
                                            type="text" 
                                            placeholder="Login" 
                                            id="register-login" 
                                            className="form-control" 
                                            value={ this.state.login } 
                                            onChange={ this.loginChange } />
                                        <div className="text-danger">{ this.state.loginMessage }</div>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-password">Hasło</label>
                                        <input 
                                            type="password" 
                                            placeholder="Hasło" 
                                            id="register-password" 
                                            className="form-control" 
                                            value={ this.state.password } 
                                            onChange={ this.passwordChange }  />
                                        <div className="text-danger">{ this.state.passwordMessage }</div>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-confirm-password">Powtórz hasło</label>
                                        <input 
                                            type="password" 
                                            placeholder="Powtórz hasło" 
                                            id="register-confirm-password" 
                                            className="form-control" 
                                            value={ this.state.confirmPassword } 
                                            onChange={ this.confirmPasswordChange }  />
                                        <div className="text-danger">{ this.state.confirmPasswordMessage }</div>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-email">Adres email</label>
                                        <input 
                                            type="text" 
                                            placeholder="Adres email" 
                                            id="register-email" 
                                            className="form-control" 
                                            value={ this.state.email } 
                                            onChange={ this.emailChange } />
                                        <div className="text-danger">{ this.state.emailMessage }</div>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="first-name-email">Imię</label>
                                        <input 
                                            type="text" 
                                            placeholder="Imię" 
                                            id="first-name-email" 
                                            className="form-control" 
                                            value={ this.state.firstName } 
                                            onChange={ this.firstNameChange } />
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-last-name">Nazwisko</label>
                                        <input 
                                            type="text" 
                                            placeholder="Nazwisko" 
                                            id="register-last-name" 
                                            className="form-control" 
                                            value={ this.state.lastName } 
                                            onChange={ this.lastNameChange } />
                                    </div>

                                    <div className="text-danger">{ this.state.message }</div>

                                    <input type="submit" className="btn btn-success float-right" value="Zarejestruj się" />
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}