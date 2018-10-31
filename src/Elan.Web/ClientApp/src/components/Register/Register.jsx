import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import './Register.css';

export default class Register extends Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: '',
            confirmPassword: '',
            email: '',
            firstName: '',
            lastName: ''
        }
    }

    loginChange = (event) => {
        this.setState({ login: event.target.value });
    }
    
    passwordChange = (event) => {
        this.setState({ password: event.target.value });
    }

    confirmPasswordChange = (event) => {
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
        console.log(this.state.login);
        console.log(this.state.password);
        console.log(this.state.confirmPassword);
        console.log(this.state.email);
        console.log(this.state.firstName);
        console.log(this.state.lastName);
        
        //do some crazy register things
    }

    render() {
        return (
            <div className="container login-page">
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