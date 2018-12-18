import React, { Component } from 'react';
import './Post.css';
import * as postsApi from '../../api/PostsApi';
import * as dateUtils from '../../utils/DateUtils';
import { getUserId } from '../../utils/JwtUtils';

export default class Post extends Component {
    constructor(props) {
        super(props);

        this.state = {
            commentsOpened: false,
            newComment: '',
            comments: [],
            commentsCount: props.commentsCount,
            reactionsCount: props.reactionsCount,
            canComment: false,
            page: 1,
            totalCount: 0,
            canLoad: true,
            showDeleteConfirmModal: false,
            deleted: false
        };
    }
    
    newCommentChange = (event) => {
        var canComment = false;
        if (event.target.value.length > 0) {
            canComment = true;
        }

        this.setState({ newComment: event.target.value, canComment });
    }

    loadComments = (page, count) => {
        postsApi.getComments(this.props.id, page, count)
            .then(function(response) {
                let sortFunc = (a, b) => new Date(a.createdOn) - new Date(b.createdOn);
                response.posts.sort(sortFunc);
                this.setState({ 
                    comments: response.posts,
                    commentsCount: response.totalCount
                 });
            }.bind(this));       
    }

    loadOlderComments = () => {
        postsApi.getComments(this.props.id, this.state.page, 10)
            .then(function (response) {
                let sortFunc = (a, b) => new Date(a.createdOn) - new Date(b.createdOn);
                response.posts.sort(sortFunc);
                this.setState({
                    comments: response.posts.concat(this.state.comments),
                    commentsCount: response.totalCount,
                    page: this.state.page + 1
                });
            }.bind(this));  
    }

    commentsModalToggle = () => {
        this.setState({commentsOpened: !this.state.commentsOpened });
        this.loadComments(0, (this.state.page) * 10);
    }

    commentPost = () => {
        if(!this.state.newComment.trim()) {
            return;
        }

        this.setState({ canComment: false });
        postsApi.commentPost(this.state.newComment, this.props.id).then(() => {
            this.setState({ newComment: '', canComment: false });
            this.loadComments(0, (this.state.page) * 10);     
        });
    }

    toggleReaction = () => {
        postsApi
            .setReaction(this.props.id)
            .then((count) => this.setState({ reactionsCount: count }));
    }

    toggleCommentReaction = (id) => {
        postsApi
            .setReaction(id)
            .then(() => 
                this.loadComments(0, (this.state.page) * 10)
            );
    }

    getPictureSource = (pictureSource) => {
        if (!pictureSource) {
            return require('./../../assets/default_avatar.jpg');
        }
        return pictureSource;
    }

    isAuthor = () => {
        if (getUserId(undefined) === this.props.userId) {
            return true;
        }

        return false;
    }

    rednerDeleteButton = () => {
        if (this.isAuthor) {
            return (
                <div className="float-right">
                    <a className="link" onClick={() => this.deleteConfirmModalToggle()}><i className="fas fa-trash-alt"></i></a>
                </div>
            );
        }
    }

    deleteConfirmModalToggle = () => {
        this.setState({ showDeleteConfirmModal: !this.state.showDeleteConfirmModal })
    }

    renderDeleteConfirmModal = () => {
        if (this.state.showDeleteConfirmModal){
            return (
                <div className="modal show delete-post" tabIndex="-1" role="dialog">
                    <div className="modal-dialog modal-sm" role="document">
                        <div className="modal-content">
                            <div className="modal-body">
                                Czy na pewno chcesz usunąć ten post?
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => this.deleteConfirmModalToggle()}>Nie</button>
                                <button className="btn btn-success" onClick={() => this.deletePost()}>Tak</button>
                        </div>
                        </div>
                    </div>
                </div>
            );
        }
    }

    deletePost = () => {
        postsApi
            .deletePost(this.props.id)
            .then(response => {
                this.deleteConfirmModalToggle();
                this.setState({ deleted: true })
            });
    }

    render() {
        let authorName = '';
        if (this.props.author !== undefined && this.props.author.trim() !== "") {
            authorName = this.props.author;
        }

        if (this.state.deleted) {
            return null;
        }

        return (
            <div className="post">
                <div className="card">
                    <div className="card-header card-sm">
                        <div className="user-info">
                            <div className="avatar-post">
                                 <img src={this.getPictureSource(this.props.pictureSource)} alt="" />
                            </div>
                            {this.rednerDeleteButton()}
                            <div className="user-post">
                                <a href={ "/account/" + this.props.userId }><strong>{ authorName }</strong></a> 
                                {this.props.to && this.props.to !== authorName && <span> do <a href={"/account/" + this.props.toUserId}><strong>{this.props.to}</strong></a></span> }
                                <br/>
                                <small className="text-muted">{dateUtils.getFormattedDate(this.props.date)}</small>
                            </div>
                        </div>
                    </div>
                    <div className="card-body">
                        <p className="card-text" dangerouslySetInnerHTML={{__html:this.props.content}}></p>
                    </div>
                    <div className="card-footer text-muted card-sm">
                        <a className="link" onClick={() => this.toggleReaction()}><i className="fas fa-beer"></i> Piwa ({this.state.reactionsCount})</a>
                        <a className="link" onClick={() => this.commentsModalToggle()}><i className="fas fa-comments"></i> Komentarze ({this.state.commentsCount})</a>
                    </div>
                </div>

                {this.renderCommentsModal()}
                {this.renderDeleteConfirmModal()}
            </div>
        );
    }

    renderCommentRow() {
        return this.state.comments.map((p,index) => (
            <span className="list-group-item list-group-item-action flex-column align-items-start post-comment" key={index}>
                <div className="row">
                    <div className="col-1">
                        <img src={this.getPictureSource(p.authorMainImageRawValue)} className="rounded float-left" alt="..."/>
                    </div>
                    <div className="col-11">
                        <div className="d-flex w-100 justify-content-between">
                            <a href={"/account/" + p.userId}><h5 className="mb-1"><strong>{p.createdBy}</strong></h5></a>
                            <small>{dateUtils.getFormattedDate(p.createdOn)}</small>
                        </div>
                        <p className="mb-1">{p.content}</p>
                        <small className="faded"><a className="link comment-link" onClick={() => this.toggleCommentReaction(p.id)}><i className="fas fa-beer"></i> Piwa ({p.reactionsCount})</a></small>
                    </div>
                </div>
            </span>)
        );
    }

    renderCommentsModal() {
        if (this.state.commentsOpened) {
            return (
                <div className="modal show post-comments" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Komentarze</h5>
                            </div>
                            <div className="modal-body">
                                {this.state.page * 10 < this.state.commentsCount && <button className="btn btn-primary" onClick={this.loadOlderComments} disabled={!this.state.canLoad}>Doczytaj starsze..</button>}
                                { this.renderCommentRow() }
                            </div>
                            <div className="modal-footer flex-column">
                                <a className="list-group-item list-group-item-action flex-column align-items-start">
                                    <textarea   rows="3" 
                                                className="form-control" 
                                                onChange={this.newCommentChange} 
                                                value={this.state.newComment}/>
                                </a>
                                <div style={{marginTop: 5}}>
                                    <button type="button" className="btn btn-info" disabled={!this.state.canComment} onClick={() => this.commentPost()}>Skomentuj</button>
                                    <button type="button" className="btn btn-secondary" onClick={() => this.commentsModalToggle()}>Zamknij</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}
