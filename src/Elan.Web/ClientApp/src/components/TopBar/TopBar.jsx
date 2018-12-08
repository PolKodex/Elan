import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import Notifications from '../Notifications/Notifications';
import { findUsers } from '../../api/UserApi';
import './TopBar.css';

class TopBar extends Component {

  constructor(props) {
    super(props);
    this.state = {
        users: []
    }
  }  


  searchChanged = (e) => {
    
    if(e.target.value === "") {
        this.setState({users: []});
        return;
    }

    findUsers(e.target.value).then((data) =>{
        this.setState({users: data});
    });
  }

  onUserClick = (id) => {
    this.props.history.push("/account/"+id);
  }

  logout = () => {
      localStorage.removeItem('token');
      window.location.reload();
  }
    getImage(image) {
        return !image
            ? require("../../assets/default_avatar.jpg")
            : image;
    }
  render() {
    let users = this.state.users.map((item, index) => (<button className="search-result" onClick={() => this.onUserClick(item.id)}>
        <img src={this.getImage(item.imageRawValue)} alt="" />
        <span>{item.firstName + " " + item.lastName}</span>
        </button>));

    if(users.length === 0) {
        users = (<button className="search-result"><span>Nothing to display</span></button>);
    }

    return (
        <div className="top-bar-wrapper">
            <div className="top-bar">
                <Link to="/" className="navbar-brand" href="#"><span className="brand-color">EL</span>AN</Link>
                <ul className="">
                    <li className="nav-item active">
                        <Link to="/" className="nav-link"><i className="fas fa-home"></i> Strona główna <span className="sr-only">(current)</span></Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/account" className="nav-link"><i className="fas fa-address-card"></i> Profil</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/settings" className="nav-link"><i className="fas fa-cog"></i> Ustawienia</Link>
                    </li>
                </ul>
                <div className="nav-right">
                    <input id="search-list" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" className="form-control mr-sm-2 form-control-sm" type="search" placeholder="Szukaj" aria-label="Search" onChange={this.searchChanged} />
                     <div className="dropdown-menu dropdown-menu-right" aria-labelledby="search-list">
                        <div className="users-search-list">
                            {users}
                       </div>
                    </div>
                    <Notifications />
                    <button className="btn btn-secondary" onClick={() => this.logout()}>Wyloguj się</button>
                </div>
            </div>
        </div>
    );
  }
}

export default withRouter(TopBar);
