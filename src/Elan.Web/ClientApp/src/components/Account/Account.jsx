import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import Post from "../Post/Post";
import './Account.css';

export default class Account extends Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {
                profilePictureSource: '../../assets/default_avatar.jpg',
                fullName: 'Maciej Owerczuk',
                description: 'Maciek jest debeściak'
            },
            friendsList: [{key: 1, pictureSource: '../../assets/no-photo.png', targetUrl: '#', title: 'Marcin Korwek'}],
            picturesList: [{key: 1, pictureSource: '../../assets/no-photo.png', targetUrl: '#', title: 'IMG1234'}],
            userPostsList: [{author: {id: 1, isGroup: false, name: "Maciej Owerczuk"}, to:{id: 2, isGroup: true, name: "Sportowe świry"}, date:"18 października o 14:34", content:"Siema ziomeczki, jak ktoś chce pograć dzisiaj w piłkę?"}]
        }
    }

    getPictureThumbnail(item, index) {
        return <PictureThumbnail 
                    key = { index }
                    pictureSource = { item.pictureSource } 
                    targetUrl = { item.targetUrl } 
                    title = { item.title } />
    }

    render() {
        //TO DO: receive user data (with all lists)

        let friendThumbnailsFirstRow = this.state.friendsList.slice(0, 4).map((item, index) => this.getPictureThumbnail(item, index));
        let friendThumbnailsSecondRow = this.state.friendsList.slice(4, 8).map((item, index) => this.getPictureThumbnail(item, index));

        let pictureThumbnailsFirstRow = this.state.picturesList.slice(0, 4).map((item, index) => this.getPictureThumbnail(item, index));
        let pictureThumbnailsSecondRow = this.state.picturesList.slice(4, 8).map((item, index) => this.getPictureThumbnail(item, index));

        let userPosts = this.state.userPostsList.map((item, index) => 
            <Post 
                author = { item.author } 
                to = { item.to } 
                content = { item.content } 
                key = { index }
                date = { item.date } />);
                
        return (
            <div>
                <div className="account-introduction">
                    <div className="media avatar">
                        <img className="align-self-start mr-3" src={ this.state.user.profilePictureSource } />
                        <div className="media-body">
                            <h3>{ this.state.user.fullName }</h3>
                            <p className="lead">{ this.state.user.description }</p>
                        </div>
                    </div>
                </div>

                <div className="card account-box">
                    <div className="card-header card-sm">
                        <a href="#">Znajomi ({ this.state.friendsList.length })</a>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            { friendThumbnailsFirstRow }
                        </div>
                        <div className="row">
                            { friendThumbnailsSecondRow }
                        </div>
                    </div>
                </div>

                <div className="card account-box">
                    <div className="card-header card-sm">
                        <a href="#">Zdjęcia ({ this.state.picturesList.length }})</a>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            { pictureThumbnailsFirstRow }
                        </div>
                        <div className="row">
                            { pictureThumbnailsSecondRow }
                        </div>
                    </div>
                </div>

                { userPosts }
            </div>
        );
    }
}

class PictureThumbnail extends Component {
    render() {
        return (
            <div className="col-md image-box">
                <a href={ this.props.targetUrl }>
                    <img src={ this.props.pictureSource } data-toggle="tooltip" data-placement="bottom" title={ this.props.title } />
                </a>
            </div>
        );
    }
}