import React, { Component } from 'react';
import './TopBar.css';

export default class TopBar extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
        <div className="top-bar">
            <a className="navbar-brand" href="#"><span className="brand-color">EL</span>AN</a>
            <ul className="">
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
            <div className="nav-right">
                <input className="form-control mr-sm-2 form-control-sm" type="search" placeholder="Szukaj" aria-label="Search" />
                <button className="btn btn-sm btn-success my-2 my-sm-0" type="submit">Szukaj</button>
            </div>
        </div>
    );
  }
}
