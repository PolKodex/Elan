import React, { Component } from 'react';
import Post from '../../components/Post/Post';
import './Wall.css';

export default class Wall extends Component {
  constructor(props) {
    super(props);

    this.state = {
      postContent: ''
    }
  }

  postContentChange = (event) => {
    this.setState({ postContent: event.target.value });
  }
  
  handleSubmit = () => {
    console.log(this.state.postContent);
    
    //do some crazy register things 
  }

  render() {
    //test
    let data =
      [{ author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: { id: 2, isGroup: true, name: "Sportowe świry" }, date: "18 października o 14:34", content: "Siema ziomeczki, jak ktoś chce pograć dzisiaj w piłkę z prawdziwymi koksami niech wpada o 20:00 na Zachodnią!!! pozdro" },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: null, date: "17 października o 14:34", content: "Zaraz godzina 23 a ja wciąż kodzę xd" },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: { id: 2, isGroup: false, name: "Marcin Korwek" }, date: "16 października o 14:34", content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin facilisis nibh ac est euismod finibus. Nam nunc justo, ultrices et auctor eu, pulvinar nec lorem. Pellentesque sem sapien, semper rutrum odio ornare, luctus molestie arcu. In sollicitudin imperdiet odio, eu convallis orci vehicula eget. Duis at vehicula orci. Donec nec felis ipsum. Curabitur fringilla lectus sed purus porttitor pellentesque. Mauris ac metus ante. Sed tincidunt, mi a pulvinar dignissim, nunc lacus ultricies turpis, in cursus massa massa id nisl. Aenean laoreet erat non congue egestas. Suspendisse pulvinar risus lectus, et imperdiet est ullamcorper et. Etiam vehicula tortor vel convallis efficitur. Sed ullamcorper, justo eget sagittis varius, quam ante faucibus lorem, euismod suscipit nulla nisl et ipsum. Sed eleifend justo mauris, a suscipit metus rhoncus quis. Ut non mi tristique, varius ex a, tincidunt est." },
      { author: { id: 1, isGroup: false, name: "Maciej Owerczuk" }, to: null, date: "15 października o 14:34", content: "Zaraz godzina 23 a ja wciąż kodzę xd <br/> <img alt='image' src='https://image-store.slidesharecdn.com/f4455761-0eff-43f5-af23-cb9b19ff75b9-original.png'/>" },]

    let posts = data.map((item, index) => 
      <Post 
        author={item.author} 
        to={item.to}
        content={item.content} 
        key={index}
        date={item.date} />)
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
                <form onSubmit={ this.handleSubmit }>
                  <textarea class="form-control" rows="5" value={ this.state.postContent } onChange={ this.postContentChange }  ></textarea>
                </form>
              </div>
          </div>
        </div>

        {posts}
      </div>
    );
  }
}
