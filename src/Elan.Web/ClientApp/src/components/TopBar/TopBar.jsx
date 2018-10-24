import React, { Component } from 'react';
import './TopBar.css';

export default class TopBar extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
        <div className="layout fixed-top">
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                <div className="container">
                    <a className="navbar-brand" href="#"><span className="brand-color">EL</span>AN</a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
        
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav mr-auto">
                            <li className="nav-item active">
                                <a className="nav-link" href="index.html"><i className="fas fa-home"></i> Strona główna <span className="sr-only">(current)</span></a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="account.html"><i className="fas fa-address-card"></i> Profil</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#"><i className="fas fa-users"></i> Grupy</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#"><i className="fas fa-cog"></i> Ustawienia</a>
                            </li>
                        </ul>
                        <form className="form-inline my-2 my-lg-0">
                            <input className="form-control mr-sm-2 form-control-sm" type="search" placeholder="Szukaj" aria-label="Search" />
                            <button className="btn btn-sm btn-success my-2 my-sm-0" type="submit">Szukaj</button>
                        </form>
                    </div>
                </div>
            </nav>
        </div>
    );
  }
}
