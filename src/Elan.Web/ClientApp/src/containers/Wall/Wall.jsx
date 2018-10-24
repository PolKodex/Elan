import React, { Component } from 'react';
import Post from '../../components/Post/Post';
import './Wall.css';

export default class Wall extends Component {
  constructor(props){
    super(props);
  }

  render() {
    //test
    let data = ["Siema ziomeczki, jak ktoś chce pograć dzisiaj w piłkę z prawdziwymi koksami niech wpada o 20:00 na Zachodnią!!! pozdro",
    "Zaraz godzina 23 a ja wciąż kodzę xd", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin facilisis nibh ac est euismod finibus. Nam nunc justo, ultrices et auctor eu, pulvinar nec lorem. Pellentesque sem sapien, semper rutrum odio ornare, luctus molestie arcu. In sollicitudin imperdiet odio, eu convallis orci vehicula eget. Duis at vehicula orci. Donec nec felis ipsum. Curabitur fringilla lectus sed purus porttitor pellentesque. Mauris ac metus ante. Sed tincidunt, mi a pulvinar dignissim, nunc lacus ultricies turpis, in cursus massa massa id nisl. Aenean laoreet erat non congue egestas. Suspendisse pulvinar risus lectus, et imperdiet est ullamcorper et. Etiam vehicula tortor vel convallis efficitur. Sed ullamcorper, justo eget sagittis varius, quam ante faucibus lorem, euismod suscipit nulla nisl et ipsum. Sed eleifend justo mauris, a suscipit metus rhoncus quis. Ut non mi tristique, varius ex a, tincidunt est."]

    let posts = data.map((post, index) => <Post content={post} key={index}/>)
    return (
      <div className="wall">
        {posts}
      </div>
    );
  }
}
