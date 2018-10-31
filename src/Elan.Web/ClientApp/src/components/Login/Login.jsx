import React, { Component } from 'react';
import * as auth from '../../api/AuthApi';
import { withRouter, Link } from "react-router-dom";
import './Login.css';
import { Redirect } from 'react-router';

export default class Login extends Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: '',
            redirect: false
        }
    }

    loginChange = (event) => {
        this.setState({ login: event.target.value });
    }
    
    passwordChange = (event) => {
        this.setState({ password: event.target.value });
    }

    handleSubmit = () => {
        auth.signIn(this.state.login, this.state.password)
            .then(function(token) {
                localStorage.setItem('token', token);
                this.setState({ redirect: true });
            }.bind(this));
    }

    render() {
        if (this.state.redirect) {
            return (<Redirect to='/app'/>);
        }
        return (
            <div className="container login-page">
                <div className="row login-logo">
                    <div className="">

                    </div>
                </div>

                <div className="row login-main">
                    <div className="col-md login-form">
                        <div className="card">
                            <div className="card-header">
                                Logowanie do portalu <strong><span className="brand-color">EL</span>AN</strong>
                            </div>
                            <div className="card-body">
                                <form onSubmit={ this.handleSubmit }>
                                    <div className="form-group">
                                        <label htmlFor="login-email">Email</label>
                                        <input 
                                            type="text" 
                                            placeholder="Email" 
                                            id="login-email" 
                                            className="form-control" 
                                            value={ this.state.login } 
                                            onChange={ this.loginChange } />
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="login-password">Hasło</label>
                                        <input 
                                            type="password" 
                                            placeholder="Hasło" 
                                            id="login-password" 
                                            className="form-control" 
                                            value={ this.state.password } 
                                            onChange={ this.passwordChange }  />
                                    </div>

                                    <a href="#" className="text-muted">Zapomniałem hasła</a>
                                    <input type="submit" className="btn btn-success float-right" value="Zaloguj się" />
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md register-welcome">
                        <div className="title text-center">
                            <p className="h3 brand-color">
                                Zarejestruj się!
                            </p>
                        </div>

                        <p className="lead">
                            Korzyści z zarejestrowania się w naszym portalu społecznościowym jest wiele, oto zaledwie kilka z nich:
                        </p>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                nawiążesz nowe znajomości
                            </li>
                            <li className="list-group-item">
                                możesz dzielić się zdjęciami i informacjami ze znajomymi
                            </li>
                            <li className="list-group-item">
                                łatwe rozmowy ze wszystkimi gdziekolwiek jesteś
                            </li>
                            <li className="list-group-item">
                                jest darmowe
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        );
    }
}
