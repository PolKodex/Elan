import React, { Component } from 'react';
import Post from "../Post/Post";
import PictureThumbnail from "../PictureThumbnail/PictureThumbnail";
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
    componentDidMount() {
        this.getData();
    }

    componentWillReceiveProps(nextProps) {
        this.setState({ userId: nextProps.match.params.id }, () => this.getData());
    }

    getData = () => {
        let userId = jwtUtils.getUserId(this.state);
        accountApi.getUser(userId)
            .then(function (response) {
                this.setState({
                    user: response.data,
                    firstNameEdit: response.data.firstName,
                    lastNameEdit: response.data.lastName,
                    descriptionEdit: response.data.description,
                    ageEdit: response.data.age,
                    isPrivate: response.data.isPrivate,
                    isFriend: response.data.isFriend,
                    invitedByMe: response.data.invitedByMe,
                    invitedMe: response.data.invitedMe
                });

                if (!response.data.isPrivate) {
                    friendsApi.getFriends(userId)
                        .then(function(response) {
                            this.setState({ friendsList: response.data });
                        }.bind(this));

                    accountApi.getUserPictures(userId)
                        .then(function(response) {
                            this.setState({ picturesList: response.data });
                        }.bind(this));

                    accountApi.getUserPosts(userId, 0, 10)
                        .then(function(response) {
                            this.setState({ userPostsList: response.data });
                        }.bind(this));
                }
            }.bind(this));
    }

    //move to utils? yes pls
    getPictureThumbnail = (index, id, pictureSource, baseUrl, title) => {
        return <PictureThumbnail
            key={index}
            pictureSource={pictureSource}
            targetUrl={baseUrl + id}
            title={title} />;
    }

    getMainPicture = () => {
        return !this.state.user.mainImage
            ? require('./../../assets/default_avatar.jpg')
            : this.state.user.mainImage.rawValue;
    }

    uploadImage = () => {
        userApi.uploadImage(this.state.mainPictureUpload, 1)
            .then(function (response) {
                var user = this.state.user; 
                var picturesList = this.state.picturesList;

                user.mainImage.rawValue = response.rawValue;
                user.mainImage.id = response.id;
                picturesList.push(response);
                this.setState({
                    mainPictureUpload: response.rawValue,
                    picturesList,
                    user,
                    showMainPictureModal: false
                });
            }.bind(this));
    }

    mainPictureChange = (event) => {
        var reader = new FileReader();
        var file = event.target.files[0];
        var self = this;

        reader.onload = function (upload) {
            self.setState({
                mainPictureUpload: upload.target.result
            }, function () {
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
                    <a href={'app/account/' + this.state.friendsList[i].id}>
                        {this.state.friendsList[i].userName}
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
            return require('./../../assets/default_avatar.jpg');
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
                this.setState({
                    user: response,
                    firstNameEdit: response.firstName,
                    lastNameEdit: response.lastName,
                    descriptionEdit: response.description,
                    ageEdit: response.age,
                    editUserClick: false
                });
            }.bind(this));
    }

    inviteToFriends = () => {
        friendsApi.inviteToFriends(this.state.userId).then(() => this.setState({invitedByMe: true}));
    }

    removeFriend = () => {
        friendsApi.removeFriend(this.state.userId).then(() => window.location.reload());
    }

    acceptInvitation = () => {
        friendsApi.acceptInvitation(this.state.userId).then(() => window.location.reload());
    }

    declineInvitation = () => {
        friendsApi.declineInvitation(this.state.userId).then(() => this.setState({ invitedMe: false }));
    }

    cancelInvitation = () => {
        friendsApi.cancelInvitation(this.state.userId).then(() => this.setState({ invitedByMe: false }));
    }


    renderEditButton = () => {
        if (this.state.userId === undefined || 
            (this.state.userId !== undefined && this.state.userId.trim() === "") || 
            this.state.userId === jwtUtils.decodeJwt(localStorage.getItem('token')).jti) {
            return (
                <a className="link faded edit" onClick={() => this.editUserClick()} title="Edytuj profil"><i className="fas fa-edit"></i></a>
            );
        }
    }

    renderFriendButtons = () => {
        if (this.state.userId !== undefined &&
            this.state.userId.trim() !== '') {
            if (this.state.isFriend) {
                return (
                    <button className="btn btn-danger" onClick={() => this.removeFriend()}>
                        Usun ze znajomych
                        </button>
                );
            }
            else if (this.state.invitedByMe) {
                return (
                    <button className="btn btn-secondary" onClick={() => this.cancelInvitation()}>
                        Anuluj zaproszenie
                        </button>
                );
            }
            else if (this.state.invitedMe) {
                return (
                    <div>
                        <button className="btn btn-success" onClick={() => this.acceptInvitation()}>
                            Akceptuj zaproszenie
                            </button>
                        <button className="btn btn-danger" onClick={() => this.declineInvitation()}>
                            Odrzuc zaproszenie
                            </button>
                    </div>
                );
            } else {
                return (
                    <button className="btn btn-secondary" onClick={() => this.inviteToFriends()}>
                        Dodaj do znajomych
                        </button>
                );
            }

        }
    }

    renderUploadImageThumbnail = () => {
        if (this.state.mainPictureUpload !== null && this.state.mainPictureUpload !== '') {
            return (<img className="upload-image thumbnail" src={this.state.mainPictureUpload} alt="" />);
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
                                            onChange={this.mainPictureChange} />
                                    </div>
                                </form>
                                {this.renderUploadImageThumbnail()}
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={() => this.uploadImage()}>Zapisz</button>
                                <button type="button" className="btn btn-secondary" onClick={() => this.mainImageClick()}>Anuluj</button>
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
                                    {this.renderFriendsListRows()}
                                </ul>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => this.friendsListClick()}>Anuluj</button>
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
                                {this.renderPicturesThumbnails()}
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => this.pictureListClick()}>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    renderEditUserModal() {
        let firstName = "";
        let lastName = "";
        let description = "";
        let age = "";
        if (this.state.firstNameEdit) {
            firstName = this.state.firstNameEdit;
        }
        if (this.state.lastNameEdit) {
            lastName = this.state.lastNameEdit;
        }
        if (this.state.descriptionEdit) {
            description = this.state.descriptionEdit;
        }
        if (this.state.ageEdit) {
            age = this.state.ageEdit;
        }

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
                                        value={firstName}
                                        onChange={this.firstNameChange} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-last-name">Nazwisko</label>
                                    <input id="edit-last-name"
                                        type="text"
                                        placeholder="Nazwisko"
                                        className="form-control"
                                        value={lastName}
                                        onChange={this.lastNameChange} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-description">Opis</label>
                                    <input id="edit-description"
                                        type="text"
                                        placeholder="Opis"
                                        className="form-control"
                                        value={description}
                                        onChange={this.descriptionChange} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-age">Wiek</label>
                                    <input id="edit-age"
                                        type="number"
                                        placeholder="Wiek"
                                        min="13"
                                        max="150"
                                        className="form-control"
                                        value={age}
                                        onChange={this.ageChange} />
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={() => this.editUser()}>Zapisz</button>
                                <button type="button" className="btn btn-secondary" onClick={() => this.editUserClick()}>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    render() {
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
                id = {item.id}
                userId={item.userId}
                author={item.createdBy}
                pictureSource={item.authorMainImageRawValue}
                to={item.targetUser}
                toUserId={item.targetUserId}
                content={item.content}
                reactions={item.reactions}
                reactionsCount={item.reactionsCount}
                commentsCount={item.commentsCount}
                key={index}
                date={item.createdOn} />);

        return (
            <div>
                <div className="account-introduction">
                    <div className="media avatar">
                        <img className="align-self-start mr-3 link"
                            src={this.getMainPicture()}
                            alt=""
                            onClick={() => this.mainImageClick()} />

                        <div className="media-body">
                            <h3>
                                {this.state.user.firstName} {this.state.user.lastName} 
                                {this.renderEditButton()}
                            </h3>

                            {!this.state.user.isPrivate && this.state.user.description && <p className="lead">„{this.state.user.description}”</p>}
                            {this.state.user.isPrivate && <p className="lead red-color">To konto jest prywatne.</p>}
                            {this.renderFriendButtons()}
                        </div>
                    </div>
                </div>

                {!this.state.isPrivate && <div className="card account-box">
                    <div className="card-header card-sm">
                        <span className="link" onClick={() => this.friendsListClick()}>Znajomi ({this.state.friendsList.length})</span>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            {this.state.friendsList.length > 0 ? friendThumbnailsFirstRow : (<p className="no-data">Brak znajomych</p>)}
                        </div>
                        <div className="row">
                            {friendThumbnailsSecondRow}
                        </div>
                    </div>
                </div>}

                {!this.state.isPrivate && <div className="card account-box">
                    <div className="card-header card-sm">
                        <span className="link" onClick={() => this.pictureListClick()}>Zdjęcia ({this.state.picturesList.length})</span>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            {this.state.picturesList.length > 0 ? pictureThumbnailsFirstRow : (<p className="no-data">Brak zdjęć</p>)}
                        </div>
                        <div className="row">
                            {pictureThumbnailsSecondRow}
                        </div>
                    </div>
                </div>}

                {userPosts}

                {this.renderMainPictureModal()}
                {this.renderFriendsListModal()}
                {this.renderPictureListModal()}
                {this.renderEditUserModal()}
            </div>
        );
    }
}