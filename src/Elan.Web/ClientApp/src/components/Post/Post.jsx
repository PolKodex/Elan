import React, { Component } from 'react';
import './Post.css';

export default class Post extends Component {
    constructor(props) {
        super(props);

        this.state = {
            commentsOpened: false,
        }

    }

    commentsModalToggle = () => {
        this.setState({ commentsOpened: !this.state.commentsOpened });
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
                                <small className="text-muted">{this.props.date}</small>
                            </div>
                        </div>
                    </div>
                    <div className="card-body">
                        <p className="card-text" dangerouslySetInnerHTML={{__html:this.props.content}}></p>
                    </div>
                    <div className="card-footer text-muted card-sm">
                        <a href="#"><i className="fas fa-beer"></i> Piwa (0)</a>
                        <a onClick={() => this.commentsModalToggle()}><i className="fas fa-comments"></i> Komentarze (1)</a>
                    </div>
                </div>
            </div>
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
                                <p> SOMETHING SHOULD BE HERE XDXDXDXDXD</p>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => this.commentsModalToggle()}>Zamknij</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}
