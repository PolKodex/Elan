import React, { Component } from 'react';
import * as auth from '../../api/AuthApi';
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
            firstNameMessage: '',
            lastName: '',
            lastNameMessage: '',
            message: '',
            redirect: false
        }
    }

    loginChange = (event) => {
        if (!event.target.value.trim()) {
            this.loginValid = false;
            this.setState({ loginMessage: "Login jest wymagany" });
        } else {
            this.loginValid = true;
            this.setState({ loginMessage: "" });
        }
        this.setState({ login: event.target.value });
    }

    isPasswordStrong = (value) => {
        var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})");
        return strongRegex.test(String(value));
    }
    
    passwordChange = (event) => {
        if (!event.target.value.trim()) {
            this.passwordValid = false;
            this.setState({ passwordMessage: "Hasła nie może być puste" });
        } else {
            this.passwordValid = true;
            this.setState({ passwordMessage: "" });
        }

        if(!this.isPasswordStrong(event.target.value)) {
            this.passwordValid = false;
            this.setState({ passwordMessage: "Hasła musi składać się z co najmniej 6 znaków. Zawierać co najmniej jedną dużą literę, małą literę, liczbę oraz znak specjalny (@,%,!,$)." });
        }

        if (event.target.value !== this.state.confirmPassword) {
            this.confirmPasswordValid = false;
            this.setState({ confirmPasswordMessage: "Hasła nie są takie same" });
        } else {
            this.confirmPasswordValid = true;
            this.setState({ confirmPasswordMessage: "" });
        }

        this.setState({ password: event.target.value });
    }

    confirmPasswordChange = (event) => {
        if (event.target.value !== this.state.password) {
            this.confirmPasswordValid = false;
            this.setState({ confirmPasswordMessage: "Hasła nie są takie same" });
        } else {
            this.confirmPasswordValid = true;
            this.setState({ confirmPasswordMessage: "" });
        }
        this.setState({ confirmPassword: event.target.value });
    }

    validateEmail = (email) => {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    emailChange = (event) => {
        if (!event.target.value.trim()) {
            this.emailValid = false;
            this.setState({ emailMessage: "Email jest wymagany" });
        } else {
            this.emailValid = true;
            this.setState({ emailMessage: "" });
        }

        if(!this.validateEmail(event.target.value)) {
            this.emailValid = false;
            this.setState({ emailMessage: "Wpisz poprawny adres email." });
        }

        this.setState({ email: event.target.value });
    }

    firstNameChange = (event) => {
        if(!event.target.value.trim()) {
            this.firstNameValid = false;
            this.setState({firstNameMessage: "Imię jest wymagane"});
        } else {
            this.firstNameValid = true;
            this.setState({firstNameMessage: ""});
        }

        this.setState({ firstName: event.target.value });
    }
    
    lastNameChange = (event) => {
        if(!event.target.value.trim()) {
            this.lastNameValid = false;
            this.setState({lastNameMessage: "Naziwsko jest wymagane"});
        } else {
            this.lastNameValid = true;
            this.setState({lastNameMessage: ""});
        }

        this.setState({ lastName: event.target.value });
    }

    handleSubmit = (event) => {
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

        event.preventDefault();
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
                loginMessage: this.state.loginMessage + value + '\n'
            });
        }
        else if (value.startsWith("Password")) {
            this.setState({ 
                passwordMessage: this.state.passwordMessage + value + '\n'
            });
        } 
        else if (value.startsWith("Email")) {
            this.setState({ 
                emailMessage: this.state.emailMessage + value + '\n'
            });
        }
        else {
            this.setState({ 
                message: this.state.message + value + '\n'
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
            return <Redirect to='/' />
        }
    }

    render() {

        let isValid = this.loginValid && this.passwordValid && this.confirmPasswordValid
                      && this.emailValid && this.firstNameValid && this.lastNameValid;

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
                                            onChange={ this.loginChange }
                                            onBlur={ this.loginChange } />
                                        <small className="form-text text-danger">{ this.state.loginMessage }</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-password">Hasło</label>
                                        <input 
                                            type="password" 
                                            placeholder="Hasło" 
                                            id="register-password" 
                                            className="form-control" 
                                            value={ this.state.password } 
                                            onChange={ this.passwordChange }
                                            onBlur={ this.passwordChange }  />
                                        <small className="form-text text-danger">{ this.state.passwordMessage }</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-confirm-password">Powtórz hasło</label>
                                        <input 
                                            type="password" 
                                            placeholder="Powtórz hasło" 
                                            id="register-confirm-password" 
                                            className="form-control" 
                                            value={ this.state.confirmPassword } 
                                            onChange={ this.confirmPasswordChange }
                                            onBlur={ this.confirmPasswordChange }  />
                                        <small className="form-text text-danger">{ this.state.confirmPasswordMessage }</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-email">Adres email</label>
                                        <input 
                                            type="email" 
                                            placeholder="Adres email" 
                                            id="register-email" 
                                            className="form-control" 
                                            onBlur={ this.emailChange } />
                                        <small className="form-text text-danger">{ this.state.emailMessage }</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="first-name-email">Imię</label>
                                        <input 
                                            type="text" 
                                            placeholder="Imię" 
                                            id="first-name-email" 
                                            className="form-control" 
                                            value={ this.state.firstName } 
                                            onChange={ this.firstNameChange }
                                            onBlur={ this.firstNameChange } />
                                        <small className="form-text text-danger">{ this.state.firstNameMessage }</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-last-name">Nazwisko</label>
                                        <input 
                                            type="text" 
                                            placeholder="Nazwisko" 
                                            id="register-last-name" 
                                            className="form-control" 
                                            value={ this.state.lastName } 
                                            onChange={ this.lastNameChange }
                                            onBlur={ this.lastNameChange } />
                                        <small className="form-text text-danger">{ this.state.lastNameMessage }</small>
                                    </div>

                                    <small className="form-text text-danger">{ this.state.message }</small>

                                    <input type="submit" className="btn btn-success float-right" value="Zarejestruj się" disabled={!isValid}/>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}