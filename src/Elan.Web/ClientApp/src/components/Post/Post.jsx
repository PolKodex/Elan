import React, { Component } from 'react';
import './Post.css';
import * as postsApi from '../../api/PostsApi';
import * as dateUtils from '../../utils/DateUtils';
import * as signalR from '@aspnet/signalr';

export default class Post extends Component {
    constructor(props) {
        super(props);

        this.state = {
            commentsOpened: false,
            newComment: '',
            comments: [],
            commentsCount: props.commentsCount,
            reactionsCount: props.reactionsCount
        }
    }
    
    newCommentChange = (event) => {
        this.setState({ newComment: event.target.value });
    }

    loadComments = () => {
        postsApi.getComments(this.props.id, 0, 10)
            .then(function(response) {
                let sortFunc = (a, b) => new Date(a.createdOn) - new Date(b.createdOn);
                response.sort(sortFunc);
                this.setState({ 
                    comments: response,
                    commentsCount: response.length
                 });
            }.bind(this));       
    }

    commentsModalToggle = () => {
        this.setState({commentsOpened: !this.state.commentsOpened });
        this.loadComments();
    }

    commentPost = () => {
        if(!this.state.newComment.trim()) {
            return;
        }

        postsApi.commentPost(this.state.newComment, this.props.id).then(() => {
            this.setState({ newComment: '' });
            this.loadComments();       
        });

    }

    addReaction = () => {

    }

    toggleReaction = () => {
        postsApi
            .setReaction(this.props.id)
            .then((count) => this.setState({ reactionsCount: count }));

    }

    toggleCommentReaction = (id) => {
        postsApi
            .setReaction(id)
            .then((count) => 
                this.loadComments()
            );
    }

    getPictureSource = (pictureSource) => {
        if (!pictureSource) {
            return require('./../../assets/default_avatar.jpg');
        }
        return pictureSource;
    }

    render() {
        let authorName = '';
        if (this.props.author !== undefined && this.props.author.trim() !== "") {
            authorName = this.props.author;
        }

        return (
            <div className="post">
                <div className="card">
                    <div className="card-header card-sm">
                        <div className="user-info">
                            <div className="avatar-post">
                                 <img src={this.getPictureSource(this.props.pictureSource)} alt="" />
                            </div>
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
                        <small className="faded"><a className="link" onClick={() => this.toggleCommentReaction(p.id)}><i className="fas fa-beer"></i> Piwa ({p.reactionsCount})</a></small>
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
                                    <button type="button" className="btn btn-info" onClick={() => this.commentPost()}>Skomentuj</button>
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
