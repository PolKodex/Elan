import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import './FooterBar.css';

class FooterBar extends Component {
    render() {
        return (
            <div className="footer-bar-wrapper">
                <div className="footer-bar">
                    <ul className="">
                        <li className="nav-item active">© 2018 PolKodex, Inc.</li>
                    </ul>
                    <div className="nav-right">
                        <ul className="">
                            <li className="nav-item">
                                <a href="https://github.com/PolKodex/Elan/issues/new/choose">Do you see a bug or want to request a change? Let us know!</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        );
    }
}

export default withRouter(FooterBar);
