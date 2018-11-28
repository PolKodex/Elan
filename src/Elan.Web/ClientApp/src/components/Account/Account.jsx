import React, { Component } from 'react';
import Post from "../Post/Post";
import * as accountApi from '../../api/AccountApi';
import * as userApi from '../../api/UserApi';
import * as friendsApi from '../../api/FriendsApi';
import * as jwtUtils from '../../utils/JwtUtils';
import './Account.css';

export default class Account extends Component {
    constructor(props) {
        super(props);

        //after testing remove sample data in state in contructor
        this.state = {
            user: {},
            friendsList: [],
            picturesList: [],
            userPostsList: [],
            userId: props.match.params.id,
            mainPictureUpload: '',
            showMainPictureModal: false,
            showFriendsListModal: false,
            showPictureListModal: false,
            showEditUserModal: false,
            firstNameEdit: '',
            lastNameEdit: '',
            descriptionEdit: '',
            ageEdit: 13
        }

    }

    //move to utils?
    getUserId = () => {
        if (this.state.userId === undefined || this.state.userId.trim() === "") {
            return jwtUtils.decodeJwt(localStorage.getItem('token')).jti;
        }
        return this.state.userId;
    }

    getData = () => {
        accountApi.getUser(this.getUserId())
            .then(function (response) {
                this.setState({ 
                    user: response.data,
                    firstNameEdit: response.data.firstName ,
                    lastNameEdit: response.data.lastName ,
                    descriptionEdit: response.data.description ,
                    ageEdit: response.data.age 
                })
            }.bind(this));

        friendsApi.getFriends(this.getUserId())
            .then(function (response) {
                this.setState({ friendsList: response.data })
            }.bind(this));

        accountApi.getUserPictures(this.getUserId())
            .then(function (response) {
                this.setState({ picturesList: response.data })
            }.bind(this));
            
        accountApi.getUserPosts(this.getUserId(), 0, 10)
            .then(function (response) {
                this.setState({ userPostsList: response.data })
        }.bind(this));
    }

    componentWillReceiveProps(nextProps) {
        this.setState({userId: nextProps.match.params.id}, () => this.getData());
    }
    
    //move to utils?
    getPictureThumbnail = (index, id, pictureSource, baseUrl, title) => {
        return <PictureThumbnail 
            key = { index }
            pictureSource = { pictureSource } 
            targetUrl = { baseUrl + id } 
            title = { title } />
    }

    getMainPicture = () => {
        return this.state.user.mainImage == null 
            ? require('./../../assets/default_avatar.jpg') 
            : this.state.user.mainImage.rawValue
    }

    uploadImage = () => {
        userApi.uploadImage(this.state.mainPictureUpload, 1)
            .then(function (response) {
                this.setState({ 
                    mainPictureUpload: response.rawValue,
                    showMainPictureModal: false
                })
            }.bind(this));
    }

    mainPictureChange = (event) => {
        var reader = new FileReader();
        var file = event.target.files[0];
        var self = this;

        reader.onload = function(upload) {
            self.setState({
                mainPictureUpload: upload.target.result
            }, function() {
            });
        };

        reader.readAsDataURL(file);

        this.setState({ mainPictureUpload: event.target.value });
    }

    renderFriendsListRows = () => {
        let list = [];
        for (let i = 0; i < this.state.friendsList.length; i++) {
            list.push(
                <li class="list-group-item">
                    <a href={ 'app/account/' + this.state.friendsList[i].id }>
                        { this.state.friendsList[i].userName }
                    </a>
                </li>
            );
        }

        return list;
    }

    mainImageClick = () => {
        this.setState({ 
            showMainPictureModal: !this.state.showMainPictureModal,
            mainPictureUpload: ''
        });
    }

    friendsListClick = () => {
        this.setState({ showFriendsListModal: !this.state.showFriendsListModal });
    }

    pictureListClick = () => {
        this.setState({ showPictureListModal: !this.state.showPictureListModal });
    }

    editUserClick = () => {
        this.setState({ editUserClick: !this.state.editUserClick });
    }

    renderPicturesThumbnails = () => {
        return this.state.picturesList.map((item, index) => 
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '/photos/', item.title));
    }

    getPictureSource = (source) => {
        if (source === null || source === undefined || source.trim() === "") {
            return require('./../../assets/no-photo.png');
        }

        return source;
    }

    firstNameChange = (event) => {
        this.setState({ firstNameEdit: event.target.value });
    }

    lastNameChange = (event) => {
        this.setState({ lastNameEdit: event.target.value });
    }

    descriptionChange = (event) => {
        this.setState({ descriptionEdit: event.target.value });
    }

    ageChange = (event) => {
        this.setState({ ageEdit: event.target.value });
    }

    editUser = () => {
        userApi
            .updateUser(
                this.state.user.id, 
                this.state.firstNameEdit, 
                this.state.lastNameEdit, 
                this.state.descriptionEdit, 
                this.state.ageEdit)
            .then(function (response) {
                this.setState({ editUserClick: false });
            }.bind(this));
    }


    render() {
        this.getData();

        //TO DO: endless scroll
        let friendThumbnailsFirstRow = this.state.friendsList.slice(0, 4).map((item, index) => 
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.avatarBase64), '/account/', item.userName));
        let friendThumbnailsSecondRow = this.state.friendsList.slice(4, 8).map((item, index) => 
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.avatarBase64), '/account/', item.userName));

        let pictureThumbnailsFirstRow = this.state.picturesList.slice(0, 4).map((item, index) => 
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '/photos/', item.title));
        let pictureThumbnailsSecondRow = this.state.picturesList.slice(4, 8).map((item, index) => 
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '/photos/', item.title));

        let userPosts = this.state.userPostsList.map((item, index) => 
            <Post 
                userId = { item.userId }
                author = { item.createdBy }
                pictureSource = { item.authorMainImageRawValue } 
                to = { item.targetUser } 
                content = { item.content } 
                key = { index }
                date = { item.createdOn } />);
        
        return (
            <div>
                <div className="account-introduction">
                    <div className="media avatar">
                        <img className="align-self-start mr-3 link" 
                            src={ this.getMainPicture() } 
                            alt=""
                            onClick={ () => this.mainImageClick() } />

                        <div className="media-body">
                            <h3>{ this.state.user.firstName } { this.state.user.lastName }</h3>
                            <p className="lead">„{ this.state.user.description }”</p>
                            <a className="link" onClick={ () => this.editUserClick() } title="Edytuj profil"><i className="fas fa-edit"></i></a>
                        </div>
                    </div>
                </div>

                <div className="card account-box">
                    <div className="card-header card-sm">
                        <span className="link" onClick={ () => this.friendsListClick() }>Znajomi ({ this.state.friendsList.length })</span>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            { this.state.friendsList.length > 0 ? friendThumbnailsFirstRow : (<p className="no-data">Brak znajomych</p>) }
                        </div>
                        <div className="row">
                            { friendThumbnailsSecondRow }
                        </div>
                    </div>
                </div>

                <div className="card account-box">
                    <div className="card-header card-sm">
                        <span className="link" onClick={ () => this.pictureListClick() }>Zdjęcia ({ this.state.picturesList.length })</span>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            { this.state.picturesList.length > 0 ? pictureThumbnailsFirstRow : (<p className="no-data">Brak zdjęć</p>) }
                        </div>
                        <div className="row">
                            { pictureThumbnailsSecondRow }
                        </div>
                    </div>
                </div>

                { userPosts }

                { this.renderMainPictureModal() }
                { this.renderFriendsListModal() }
                { this.renderPictureListModal() }
                { this.renderEditUserModal() }
            </div>
        );
    }

    renderUploadImageThumbnail = () => {
        if (this.state.mainPictureUpload !== null && this.state.mainPictureUpload !== '') {
            return <img className="upload-image thumbnail" src={ this.state.mainPictureUpload } alt="" />;
        }
    }

    renderMainPictureModal() {
        if (this.state.showMainPictureModal) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Załaduj zdjęcie profilowe</h5>
                            </div>
                            <div className="modal-body">
                                <form>
                                    <div className="form-group">
                                        <input type="file" 
                                            placeholder="Załaduj zdjęcie" 
                                            id="main-picture" 
                                            className="form-control-file"
                                            onChange={ this.mainPictureChange } />
                                    </div>
                                </form>
                                { this.renderUploadImageThumbnail() }
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={ () => this.uploadImage() }>Zapisz</button>
                                <button type="button" className="btn btn-secondary" onClick={ () => this.mainImageClick() }>Anuluj</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    renderFriendsListModal() {
        if (this.state.showFriendsListModal) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Lista znajomych</h5>
                            </div>
                            <div className="modal-body">
                                <ul class="list-group">
                                    { this.renderFriendsListRows() }
                                </ul>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={ () => this.friendsListClick() }>Anuluj</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    renderPictureListModal() {
        if (this.state.showPictureListModal) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Wszystkie zdjęcia</h5>
                            </div>
                            <div className="modal-body">
                                { this.renderPicturesThumbnails() }
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={ () => this.pictureListClick() }>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    renderEditUserModal() {
        if (this.state.editUserClick) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Edytuj podstawowe informacje</h5>
                            </div>
                            <div className="modal-body">
                                <div className="form-group">
                                    <label htmlFor="edit-first-name">Imię</label>
                                    <input id="edit-first-name"
                                            type="text" 
                                            placeholder="Imię" 
                                            className="form-control"
                                            value={ this.state.firstNameEdit } 
                                            onChange={ this.firstNameChange } ></input>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-last-name">Nazwisko</label>
                                    <input id="edit-last-name"
                                            type="text" 
                                            placeholder="Nazwisko" 
                                            className="form-control"
                                            value={ this.state.lastNameEdit } 
                                            onChange={ this.lastNameChange } ></input>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-description">Opis</label>
                                    <input id="edit-description"
                                            type="text" 
                                            placeholder="Opis" 
                                            className="form-control"
                                            value={ this.state.descriptionEdit } 
                                            onChange={ this.descriptionChange } ></input>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-age">Wiek</label>
                                    <input id="edit-age"
                                            type="number" 
                                            placeholder="Wiek" 
                                            min="13" 
                                            max="150" 
                                            className="form-control"
                                            value={ this.state.ageEdit } 
                                            onChange={ this.ageChange } ></input>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={ () => this.editUser() }>Zapisz</button>
                                <button type="button" className="btn btn-secondary" onClick={ () => this.editUserClick() }>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}

//move to new component?
class PictureThumbnail extends Component {
    render() {
        return (
            <div className="col-md-3 image-box">
                <a href={ this.props.targetUrl } className="thumbnail">
                    <img src={ this.props.pictureSource } data-toggle="tooltip" data-placement="bottom" title={ this.props.title } alt="" />
                </a>
            </div>
        );
    }
}