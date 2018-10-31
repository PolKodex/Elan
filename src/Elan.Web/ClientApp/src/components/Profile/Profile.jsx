import React, { Component } from 'react';
import { withRouter, Link } from "react-router-dom";
import './Profile.css';

export default class Profile extends Component {
    constructor(props) {
        super(props);
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

        let friendThumbnailsFirstRow = friendsList.slice(0, 4).map((item, index) => getPictureThumbnail(item, index));
        let friendThumbnailsSecondRow = friendsList.slice(4, 8).map((item, index) => getPictureThumbnail(item, index));

        let pictureThumbnailsFirstRow = picturesList.slice(0, 4).map((item, index) => getPictureThumbnail(item, index));
        let pictureThumbnailsSecondRow = picturesList.slice(4, 8).map((item, index) => getPictureThumbnail(item, index));

        let userPosts = userPostsList.map((item, index) => 
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
                        <img className="align-self-start mr-3" src={ require(this.props.profilePictureSource) } />
                        <div className="media-body">
                            <h3>{ this.props.fullName }</h3>
                            <p className="lead">{ this.props.description }</p>
                        </div>
                    </div>
                </div>

                <div className="card account-box">
                    <div className="card-header card-sm">
                        <a href="#">Znajomi ({ friendsList.length })</a>
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
                        <a href="#">Zdjęcia ({ pictureThumbnails.length }})</a>
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
                    <img src={ require(this.props.pictureSource) } data-toggle="tooltip" data-placement="bottom" title={ this.props.title } />
                </a>
            </div>
        );
    }
}