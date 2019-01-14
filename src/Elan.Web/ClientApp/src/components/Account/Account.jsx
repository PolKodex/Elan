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
            showValidationModal: false,
            firstNameEdit: '',
            lastNameEdit: '',
            descriptionEdit: '',
            ageEdit: 13,
            genderEdit: 0,
            genderSelected: false, 
            owner: false,
            firstNameValid: false,
            lastNameValid: false,
            ageValid: false,
            canLoad: true,
            page: 1,
            totalCount: 0
        };
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
                    genderEdit: response.data.gender > 0 ? response.data.gender : 1,
                    genderSelected: response.data.gender > 0,
                    ageValid: true,
                    firstNameValid: true,
                    lastNameValid: true,
                    isPrivate: response.data.isPrivate,
                    isFriend: response.data.isFriend,
                    invitedByMe: response.data.invitedByMe,
                    invitedMe: response.data.invitedMe
                });

                if (!response.data.isPrivate) {
                    friendsApi.getFriends(userId)
                        .then(function (response) {
                            this.setState({ friendsList: response.data });
                        }.bind(this));

                    accountApi.getUserPictures(userId)
                        .then(function (response) {
                            this.setState({ picturesList: response.data });
                        }.bind(this));

                    accountApi.getUserPosts(userId, 0, 10)
                        .then(function (postListing) {
                            let sortFunc = (a, b) => new Date(b.createdOn) - new Date(a.createdOn);
                            postListing.posts.sort(sortFunc);
                            this.setState({
                                userPostsList: postListing.posts,
                                totalCount: postListing.totalCount
                            });
                        }.bind(this));
                }
            }.bind(this));

        this.setState(
            {
                owner: this.state.userId === undefined ||
                    (this.state.userId !== undefined && this.state.userId.trim() === "") ||
                    this.state.userId === jwtUtils.decodeJwt(localStorage.getItem('token')).jti
            }
        );
    }

    //move to utils? yes pls
    getPictureThumbnail = (index, id, pictureSource, baseUrl, title) => {
        return <PictureThumbnail
            key={index}
            pictureSource={pictureSource}
            targetUrl={baseUrl.trim() !== '' ? baseUrl + id : ''}
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

                user.mainImage = response;
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
                    <a href={'account/' + this.state.friendsList[i].id}>
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
        if (this.state.editUserClick) {
            this.setState({
                firstNameEdit: this.state.user.firstName,
                lastNameEdit: this.state.user.lastName,
                ageEdit: this.state.user.age
            });
        }
        this.setState({ editUserClick: !this.state.editUserClick });
    }

    validationModalToggle = () => {
        this.setState({ showValidationModal: !this.state.showValidationModal })
    }

    renderPicturesThumbnails = () => {
        return this.state.picturesList.map((item, index) =>
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '', item.title));
    }

    getPictureSource = (source) => {
        if (source === null || source === undefined || source.trim() === "") {
            return require('./../../assets/default_avatar.jpg');
        }

        return source;
    }

    loadOlderPosts = () => {
        this.setState({ canLoad: false });
        accountApi.getUserPosts(this.state.userId, this.state.page, 10)
            .then(function (postListing) {
                let sortFunc = (a, b) => new Date(b.createdOn) - new Date(a.createdOn);
                postListing.posts.sort(sortFunc);
                this.setState(
                    {
                        userPostsList: this.state.userPostsList.concat(postListing.posts),
                        totalCount: postListing.totalCount,
                        page: this.state.page + 1,
                        canLoad: true
                    });
            }.bind(this));
    };

    firstNameChange = (event) => {
        if (!event.target.value.trim()) {
            this.setState({ firstNameValid: false });
            this.setState({ firstNameMessage: "Imię jest wymagane" });
        } else {
            this.setState({ firstNameValid: true });
            this.setState({ firstNameMessage: "" });
        }
        this.setState({ firstNameEdit: event.target.value });
    }

    lastNameChange = (event) => {
        if (!event.target.value.trim()) {
            this.setState({ lastNameValid: false });
            this.setState({ lastNameMessage: "Nazwisko jest wymagane" });
        } else {
            this.setState({ lastNameValid: true });
            this.setState({ lastNameMessage: "" });
        }
        this.setState({ lastNameEdit: event.target.value });
    }

    descriptionChange = (event) => {
        this.setState({ descriptionEdit: event.target.value });
    }

    genderChange = (event) => {
        this.setState({ genderEdit: event.target.value });
    }

    ageChange = (event) => {
        const re = /^[0-9\b]+$/;
        if (event.target.value.trim() && (!re.test(event.target.value) || event.target.value < 13 || event.target.value > 150)) {
            this.setState({ ageValid: false });
            this.setState({ ageMessage: "Niepoprawna wartość" });
        } else {
            this.setState({ ageValid: true });
            this.setState({ ageMessage: "" });
        }
        this.setState({ ageEdit: event.target.value });
    }

    editUser = () => {
        if (this.state.ageEdit < 0) {
            this.setState({
                showValidationModal: true
            });
        } else {
            userApi
                .updateUser(
                    this.state.user.id,
                    this.state.firstNameEdit,
                    this.state.lastNameEdit,
                    this.state.descriptionEdit,
                    this.state.ageEdit,
                    this.state.genderEdit)
                .then(function (response) {
                    this.setState({
                        user: response,
                        firstNameEdit: response.firstName,
                        lastNameEdit: response.lastName,
                        descriptionEdit: response.description,
                        ageEdit: response.age,
                        genderSelected: true,
                        genderEdit : response.gender,
                        editUserClick: false
                    });
                }.bind(this));
        }
    }

    inviteToFriends = () => {
        friendsApi.inviteToFriends(this.state.userId).then(() => this.setState({ invitedByMe: true }));
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
        if (this.state.owner) {
            return (
                <a className="link faded edit" onClick={() => this.editUserClick()} title="Edytuj profil"><i className="fas fa-edit"></i></a>
            );
        }
    }

    renderProfilePicture = () => {
        if (this.state.owner) {
            return (
                <img className="align-self-start mr-3 link"
                    src={this.getMainPicture()}
                    alt=""
                    onClick={() => this.mainImageClick()} />
            );
        }
        else {
            return (
                <img className="align-self-start mr-3"
                    src={this.getMainPicture()}
                    alt="" />
            );
        }
    }

    renderFriendButtons = () => {
        if (!this.state.owner) {
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

    renderValidationModal() {
        if (this.state.showValidationModal) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-body">
                                <form>
                                    <img className="upload-image thumbnail" src={require('./../../assets/validation.jpg')} />
                                </form>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => this.validationModalToggle()}>Wróć i popraw dane</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
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
                                <div className="row">
                                    {this.renderPicturesThumbnails()}
                                </div>
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
        let gender = 1;
        let genderSelected = false;
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
        if (this.state.genderEdit) {
            gender = this.state.genderEdit;
        }

        if (this.state.genderSelected) {
            genderSelected = true;
        }

        let isValid = this.state.firstNameValid && this.state.lastNameValid && this.state.ageValid;

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
                                    <small className="form-text text-danger">{this.state.firstNameMessage}</small>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-last-name">Nazwisko</label>
                                    <input id="edit-last-name"
                                        type="text"
                                        placeholder="Nazwisko"
                                        className="form-control"
                                        value={lastName}
                                        onChange={this.lastNameChange} />
                                    <small className="form-text text-danger">{this.state.lastNameMessage}</small>
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
                                        type="text"
                                        pattern="[0-9]*"
                                        placeholder="Wiek"
                                        className="form-control"
                                        value={age}
                                        onChange={this.ageChange} />
                                    <small className="form-text text-danger">{this.state.ageMessage}</small>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="edit-age">Płeć</label>
                                    <select className="form-control" value={gender} onChange={this.genderChange} disabled={genderSelected}>
                                        <option value="1">Mężczyzna</option>
                                        <option value="2">Kobieta</option>
                                    </select>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={() => this.editUser()} disabled={!isValid}>Zapisz</button>
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
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '', item.title));
        let pictureThumbnailsSecondRow = this.state.picturesList.slice(4, 8).map((item, index) =>
            this.getPictureThumbnail(index, item.id, this.getPictureSource(item.rawValue), '', item.title));

        let userPosts = this.state.userPostsList.map((item, index) =>
            <Post
                id={item.id}
                userId={item.userId}
                author={item.createdBy}
                pictureSource={item.authorMainImageRawValue}
                to={item.targetUser}
                toUserId={item.targetUserId}
                content={item.content}
                reactions={item.reactions}
                reactionsCount={item.reactionsCount}
                commentsCount={item.commentsCount}
                key={item.id}
                date={item.createdOn} />);

        return (
            <div>
                <div className="account-introduction">
                    <div className="media avatar">
                        {this.renderProfilePicture()}

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
                <div className="text-center">
                    {this.state.page * 10 < this.state.totalCount && <button className="btn btn-link" onClick={this.loadOlderPosts} disabled={!this.state.canLoad}>Pokaż więcej</button>}
                </div>

                {this.renderMainPictureModal()}
                {this.renderFriendsListModal()}
                {this.renderPictureListModal()}
                {this.renderEditUserModal()}
                {this.renderValidationModal()}
            </div>
        );
    }
}