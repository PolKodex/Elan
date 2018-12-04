import React, { Component } from 'react';
import './Post.css';
import * as postsApi from '../../api/PostsApi';
import * as dateUtils from '../../utils/DateUtils';

export default class Post extends Component {
    constructor(props) {
        super(props);

        this.state = {
            commentsOpened: false,
            newComment: '',
            comments: [],
            commentsCount: 0,
            reactionsCount: 0
        }

    }

    newCommentChange = (event) => {
        this.setState({ newComment: event.target.value });
    }

    commentsModalToggle = () => {
        postsApi
            .getComments(this.props.id, 0, 10)
            .then(function(response) {
                this.setState({ 
                    comments: response,
                    commentsCount: response.length,
                    commentsOpened: !this.state.commentsOpened 
                 });
            }.bind(this));
    }

    commentPost = () => {
        postsApi.commentPost(this.state.newComment, this.props.id);
        this.setState({ newComment: '' });
    }

    addReaction = () => {

    }

    toggleReaction = () => {
        postsApi.setReaction(this.props.id);
    }

    render() {
        let authorName = '';
        if (this.props.author !== undefined && this.props.author.trim() !== "") {
            authorName = this.props.author;
        }

        let pictureSource = this.props.pictureSource;

        if (!pictureSource) {
            pictureSource = "../../assets/default_avatar.jpg";
        }

        return (
            <div className="post">
                <div className="card">
                    <div className="card-header card-sm">
                        <div className="user-info">
                            <div className="avatar-post">
                                 <img src={pictureSource} alt="" />
                            </div>
                            <div className="user-post">
                                <a href={ "/account/" + this.props.userId }><strong>{ authorName }</strong></a> 
                                {this.props.to && this.props.to.isGroup ? ' w ' : ""}
                                {this.props.to && !this.props.to.isGroup ? ' do ' : ""}
                                <a href="#"><strong>{this.props.to ? this.props.to.name : ""}</strong></a> 
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
        return this.state.comments.map(p => (
            <a href="#" className="list-group-item list-group-item-action flex-column align-items-start">
                <div className="d-flex w-100 justify-content-between">
                    <h5 className="mb-1"><strong>{p.createdBy}</strong></h5>
                    <small>{dateUtils.getFormattedDate(p.createdOn)}</small>
                </div>
                <p className="mb-1">{p.content}</p>
            </a>)
        );
    }

    renderCommentsModal() {
        if (this.state.commentsOpened) {
            return (
                <div className="modal show" tabIndex="-1" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Komentarze</h5>
                            </div>
                            <div className="modal-body">
                                { this.renderCommentRow() }
                                <a href="#" className="list-group-item list-group-item-action flex-column align-items-start">
                                    <textarea   rows="3" 
                                                className="form-control" 
                                                onChange={this.newCommentChange} 
                                                value={this.newComment}/>
                                </a>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-info" onClick={() => this.commentPost()}>Skomentuj</button>
                                <button type="button" className="btn btn-secondary" onClick={() => this.commentsModalToggle()}>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}
