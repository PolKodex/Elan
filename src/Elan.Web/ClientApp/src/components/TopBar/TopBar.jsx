import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import './TopBar.css';

class TopBar extends Component {
  render() {
    return (
        <div className="top-bar-wrapper">
            <div className="top-bar">
                <Link to="/app" className="navbar-brand" href="#"><span className="brand-color">EL</span>AN</Link>
                <ul className="">
                    <li className="nav-item active">
                        <Link to="/app" className="nav-link"><i className="fas fa-home"></i> Strona główna <span className="sr-only">(current)</span></Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/app/account" className="nav-link"><i className="fas fa-address-card"></i> Profil</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/app/settings" className="nav-link"><i className="fas fa-cog"></i> Ustawienia</Link>
                    </li>
                </ul>
                <div className="nav-right">
                    <input className="form-control mr-sm-2 form-control-sm" type="search" placeholder="Szukaj" aria-label="Search" />
                    <button className="btn btn-sm btn-success my-2 my-sm-0" type="submit">Szukaj</button>
                </div>
            </div>
        </div>
    );
  }
}

export default withRouter(TopBar);
