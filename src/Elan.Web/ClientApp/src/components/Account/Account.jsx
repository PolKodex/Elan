import React, { Component } from 'react';
import Post from "../Post/Post";
import * as accountApi from '../../api/AccountApi';
import * as jwtUtils from '../../utils/JwtUtils';
import './Account.css';

export default class Account extends Component {
    constructor(props) {
        super(props);

        //after testing remove sample data in state in contructor
        this.state = {
            user: {
                id: 'guid-12345678-abcdefgh',
                profilePictureSource: '../../assets/default_avatar.jpg',
                fullName: 'Maciej Owerczuk',
                description: 'Maciek jest debeściak'
            },
            friendsList: [{key: 1, pictureSource: '../../assets/no-photo.png', targetUrl: '#', title: 'Marcin Korwek'}],
            picturesList: [{key: 1, pictureSource: '../../assets/no-photo.png', targetUrl: '#', title: 'IMG1234'}],
            userPostsList: [
                {
                    author: 
                    {
                        id: 1, 
                        isGroup: false, 
                        name: "Maciej Owerczuk"
                    }, 
                    to:
                    {
                        id: 2, 
                        isGroup: true, 
                        name: "Sportowe świry"
                    }, 
                    date: "18 października o 14:34", 
                    content: "Siema ziomeczki, jak ktoś chce pograć dzisiaj w piłkę?"
                }],
            userId: props.match.params.id
        }

    }

    getUserId() {
        if (this.state.userId == undefined || this.state.userId.trim() == "") {
            return jwtUtils.decodeJwt(localStorage.getItem('token')).jti;
        }
        return this.state.userId;
    }

    getData = () => {
        accountApi.getUser(this.getUserId())
            .then(function (response) {
                this.setState({ user: response.data })
            }.bind(this));

        accountApi.getUserFriends()
            .then(function (response) {
                this.setState({ friendsList: response.data })
            }.bind(this));

        accountApi.getUserPictures()
            .then(function (response) {
                this.setState({ picturesList: response.data })
            }.bind(this));
            
        accountApi.getUserPosts(jwtUtils.decodeJwt(localStorage.getItem('token')).jti, 0, 10)
        .then(function (response) {
            this.setState({ userPostsList: response.data })
        }.bind(this));        
    }
    
    componentDidMount() {
        this.getData();
    }

    componentWillReceiveProps(nextProps) {
        this.setState({userId: nextProps.match.params.id}, () => this.getData());
    }
    
    getPictureThumbnail(index, id, pictureSource, baseUrl, title) {
        return <PictureThumbnail 
            key = { index }
            pictureSource = { pictureSource } 
            targetUrl = { baseUrl + id } 
            title = { title } />
    }

    render() {
        //TO DO: endless scroll
        let friendThumbnailsFirstRow = this.state.friendsList.slice(0, 4).map((item, index) => 
            this.getPictureThumbnail(index, item.id, './../../assets/default_avatar.jpg', '/app/account/', item.userName));
        let friendThumbnailsSecondRow = this.state.friendsList.slice(4, 8).map((item, index) => 
            this.getPictureThumbnail(index, item.id, './../../assets/default_avatar.jpg', '/app/account/', item.userName));

        let pictureThumbnailsFirstRow = this.state.picturesList.slice(0, 4).map((item, index) => 
            this.getPictureThumbnail(index, item.id, './../../assets/no-photo.png', '/app/photos/', item.title));
        let pictureThumbnailsSecondRow = this.state.picturesList.slice(4, 8).map((item, index) => 
            this.getPictureThumbnail(index, item.id, './../../assets/no-photo.png', '/app/photos/', item.title));

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
                        <img className="align-self-start mr-3" src={ this.state.user.profilePictureSource } alt="" />
                        <div className="media-body">
                            <h3>{ this.state.user.firstName } { this.state.user.lastName }</h3>
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
                        <a href="#">Zdjęcia ({ this.state.picturesList.length })</a>
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
                    <img src={require('./../../assets/no-photo.png')} data-toggle="tooltip" data-placement="bottom" title={ this.props.title } alt="" />
                </a>
            </div>
        );
    }
}