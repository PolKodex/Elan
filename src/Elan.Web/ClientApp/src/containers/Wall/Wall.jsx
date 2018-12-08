import React, { Component } from 'react';
import Post from '../../components/Post/Post';
import { savePost, getLatestPosts } from '../../api/PostsApi';
import './Wall.css';

export default class Wall extends Component {
    constructor(props) {
        super(props);

        this.state = {
            postContent: '',
            posts: []
        }
    }

    componentDidMount() {
        this.getPosts();
    }

    componentWillUnmount() {
        if (this.interval)
            clearInterval(this.interval);
    }

    getPosts = () => {
        getLatestPosts().then((posts) => {
            let sortFunc = (a, b) => new Date(b.createdOn) - new Date(a.createdOn);
            posts.sort(sortFunc);
            this.setState({ posts });
        });
    }

    postContentChange = (event) => {
        this.setState({ postContent: event.target.value });
    }

    post = () => {
        savePost(this.state.postContent)
            .then(() => {
                this.setState({ postContent: "" })
                this.getPosts();
            });
    };

    render() {
        let posts = this.state.posts.map((item, index) =>
            <Post
                id={item.id}
                userId={item.userId}
                author={item.createdBy}
                pictureSource={item.authorMainImageRawValue}
                to={item.to}
                content={item.content}
                reactions={item.reactions}
                reactionsCount={item.reactionsCount}
                commentsCount={item.commentsCount}
                key={index}
                date={item.createdOn} />)

        return (
            <div className="wall">
                <div className="post">
                    <div className="card">
                        <div className="card-header card-sm">
                            <div className="post-header">
                                <strong>Napisz co u Ciebie słychać</strong>
                            </div>
                        </div>
                        <div className="card-body">
                            <textarea className="form-control" rows="5" value={this.state.postContent} onChange={this.postContentChange}  ></textarea>
                            <button className="btn btn-sm btn-success my-2 my-sm-0" onClick={this.post}>POST</button>
                        </div>
                    </div>
                </div>

                {posts}
            </div>
        );
    }
}
