import React, { Component } from 'react';
import './Post.css';

export default class Post extends Component {
    render() {
        let authorName = '';
        if (this.props.author.name !== undefined && this.props.author.name.trim() !== "") {
            authorName = this.props.author.name;
        }

        return (
            <div className="post">
                <div className="card">
                    <div className="card-header card-sm">
                        <div className="user-info">
                            <div className="avatar-post">
                                <img src={require("../../assets/default_avatar.jpg")} alt="" />
                            </div>
                            <div className="user-post">
                                <a href="#"><strong>{ authorName }</strong></a> 
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
                        <a href="#"><i className="fas fa-comments"></i> Komentarze (1)</a>
                    </div>
                </div>
            </div>
        );
    }
}
