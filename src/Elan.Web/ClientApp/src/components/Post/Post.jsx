import React, { Component } from 'react';
import './Post.css';

export default class Post extends Component {

    formatDate = (date) => {
    return `${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
    };

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
                                <small className="text-muted">{this.formatDate(new Date(this.props.date))}</small>
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
