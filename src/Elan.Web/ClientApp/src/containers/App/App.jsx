import React, { Component } from 'react';
import { Route } from 'react-router';
import TopBar from '../../components/TopBar/TopBar';
import Chat from '../Chat/Chat';
import './App.css';

export default class App extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
      <div className="app">
        <TopBar />
        <div className="app-view">
          <div className="page">

          </div>
          <div className="chat-container">
            <Chat />
          </div>
        </div>
      </div>
    );
  }
}
