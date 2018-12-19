import React, { Component } from 'react';
import * as auth from '../../api/AuthApi';
import { Redirect } from 'react-router';

export default class RemindPassword extends Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            loginMessage: '',
            password: '',
            passwordMessage: '',
            confirmPassword: '',
            confirmPasswordMessage: '',
            question: '',
            answer: '',
            answerMessage: '',
            message: '',
            redirect: false
        }
    }

    loginChange = (event, blur) => {
        if (!event.target.value.trim()) {
            this.loginValid = false;
            this.setState({ loginMessage: "Login jest wymagany", showQuestion: false });
        } else {
            this.loginValid = true;
            if (blur) {
                auth.getHintQuestion(
                        event.target.value)
                    .then(function(question) {
                        this.setState({ question: question });
                    }.bind(this))
                    .catch((response) => {
                        this.loginValid = false;
                        this.setState({ question: false });
                        this.handleErrors(response);
                    });
            } else {
                this.setState({ question: false });
            }

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

        if (!this.isPasswordStrong(event.target.value)) {
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

    answerChange = (event) => {
        if (!event.target.value.trim()) {
            this.answerValid = false;
            this.setState({ answerMessage: "Odpowiedź na pytanie pomocnicze jest wymagana" });
        } else {
            this.answerValid = true;
            this.setState({ answerMessage: "" });
        }

        this.setState({ answer: event.target.value });
    }

    handleSubmit = (event) => {
        auth.changePassword(
            this.state.login,
            this.state.password,
            this.state.answer)
            .then(function (token) {
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

        if (value.startsWith("Password")) {
            this.setState({
                passwordMessage: this.state.passwordMessage + value + '\n'
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

        let isValid = this.passwordValid && this.confirmPasswordValid
            && this.answerValid && this.loginValid;

        return (
            <div className="container login-page">
                {this.renderRedirect()}
                <div className="row login-logo">
                    <div className="">

                    </div>
                </div>

                <div className="row login-main">
                    <div className="offset-md-2 col-md-8 login-form">
                        <div className="card">
                            <div className="card-header">
                                Zmiana hasła do portalu <strong><span className="brand-color">EL</span>AN</strong>
                            </div>
                            <div className="card-body">
                                <form onSubmit={this.handleSubmit}>
                                    <div className="form-group">
                                        <label htmlFor="register-login">Login</label>
                                        <input
                                            type="text"
                                            placeholder="Login"
                                            id="register-login"
                                            className="form-control"
                                            value={this.state.login}
                                            onChange={(e) => this.loginChange(e, false)}
                                            onBlur={(e) => this.loginChange(e, true)} />
                                        <small className="form-text text-danger">{this.state.loginMessage}</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-password">Hasło</label>
                                        <input
                                            type="password"
                                            placeholder="Hasło"
                                            id="register-password"
                                            className="form-control"
                                            value={this.state.password}
                                            onChange={this.passwordChange}
                                            onBlur={this.passwordChange} />
                                        <small className="form-text text-danger">{this.state.passwordMessage}</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-confirm-password">Powtórz hasło</label>
                                        <input
                                            type="password"
                                            placeholder="Powtórz hasło"
                                            id="register-confirm-password"
                                            className="form-control"
                                            value={this.state.confirmPassword}
                                            onChange={this.confirmPasswordChange}
                                            onBlur={this.confirmPasswordChange} />
                                        <small className="form-text text-danger">{this.state.confirmPasswordMessage}</small>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="register-question">Pytanie pomocnicze: {this.state.question}</label>
                                        <input
                                            type="text"
                                            placeholder="Odpowiedź na pytanie pomocnicze"
                                            id="register-answer"
                                            className="form-control"
                                            value={this.state.answer}
                                            onChange={this.answerChange}
                                            onBlur={this.answerChange} />
                                        <small className="form-text text-danger">{this.state.answerMessage}</small>
                                    </div>


                                    <small className="form-text text-danger">{this.state.message}</small>

                                    <input type="submit" className="btn btn-success float-right" value="Zmień hasło" disabled={!isValid} />
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}