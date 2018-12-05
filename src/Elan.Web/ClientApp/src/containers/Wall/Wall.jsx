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

  componentDidMount(){
      this.getPosts();
  }

  componentWillUnmount() {
    if(this.interval)
    clearInterval(this.interval);
  }

  getPosts = () => {
    getLatestPosts().then((posts) => {
      let sortFunc = (a,b) => new Date(b.createdOn) - new Date(a.createdOn);
      posts.sort(sortFunc);
      this.setState({posts});
    });
  }

  postContentChange = (event) => {
    this.setState({ postContent: event.target.value });
  }
  
  handleSubmit = () => {
    
    //do some crazy register things 
  }

  post = () => {
    savePost(this.state.postContent)
      .then(() => {
        this.setState({postContent: ""})
        this.getPosts();
        });
  };

  render() {
    /*
    let data =
      [{ author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: { id: 2, isGroup: true, name: "Sportowe świry" }, date: "18 października o 14:34", content: "Siema ziomeczki, jak ktoś chce pograć dzisiaj w piłkę z prawdziwymi koksami niech wpada o 20:00 na Zachodnią!!! pozdro" },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: null, date: "17 października o 14:34", content: "Zaraz godzina 23 a ja wciąż kodzę xd" },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: { id: 2, isGroup: false, name: "Marcin Korwek" }, date: "16 października o 14:34", content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin facilisis nibh ac est euismod finibus. Nam nunc justo, ultrices et auctor eu, pulvinar nec lorem. Pellentesque sem sapien, semper rutrum odio ornare, luctus molestie arcu. In sollicitudin imperdiet odio, eu convallis orci vehicula eget. Duis at vehicula orci. Donec nec felis ipsum. Curabitur fringilla lectus sed purus porttitor pellentesque. Mauris ac metus ante. Sed tincidunt, mi a pulvinar dignissim, nunc lacus ultricies turpis, in cursus massa massa id nisl. Aenean laoreet erat non congue egestas. Suspendisse pulvinar risus lectus, et imperdiet est ullamcorper et. Etiam vehicula tortor vel convallis efficitur. Sed ullamcorper, justo eget sagittis varius, quam ante faucibus lorem, euismod suscipit nulla nisl et ipsum. Sed eleifend justo mauris, a suscipit metus rhoncus quis. Ut non mi tristique, varius ex a, tincidunt est." },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: null, date: "15 października o 14:34", content: "Zaraz godzina 23 a ja wciąż kodzę xd <br/> <img alt='image' src='https://image-store.slidesharecdn.com/f4455761-0eff-43f5-af23-cb9b19ff75b9-original.png'/>" },]
    */

    let posts = this.state.posts.map((item, index) => 
      <Post 
        id = { item.id }
        userId = { item.userId }
        author = { item.createdBy } 
        pictureSource = { item.authorMainImageRawValue } 
        to = { item.to }
        content = { item.content } 
        reactions={item.reactions}
        key = { index }
        date = { item.createdOn } />)

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
                <textarea className="form-control" rows="5" value={ this.state.postContent } onChange={ this.postContentChange }  ></textarea>
                <button className="btn btn-sm btn-success my-2 my-sm-0" onClick={this.post}>POST</button>
              </div>
          </div>
        </div>

        {posts}
      </div>
    );
  }
}
