import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import './Login.css';

class Login extends Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: ''
        }
    }

    loginChange = () => {
        this.setState({ login: event.target.value });
    }
    
    passwordChange = () => {
        this.setState({ password: event.target.value });
    }

    handleSubmit = () => {
        console.log(this.state.login);
        console.log(this.state.password);
        
        //do some crazy login things
    }

    render() {
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
                                        <label for="login-email">Email</label>
                                        <input 
                                            type="text" 
                                            placeholder="Email" 
                                            id="login-email" 
                                            className="form-control" 
                                            value={ this.state.login } 
                                            onChange={ this.loginChange } />
                                    </div>

                                    <div className="form-group">
                                        <label for="login-password">Hasło</label>
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
                            Korzyści z zarejestrowania się w naszym poratlu społecznościowym jest wiele, oto zaledwie kilka z nich:
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