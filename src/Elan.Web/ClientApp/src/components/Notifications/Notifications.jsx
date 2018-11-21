import React, { Component } from 'react';
import './Notifications.css';
import { getNotifications, setNotificationAsRead } from '../../api/NotificationsApi';

export default class Notifications extends Component {

  constructor(props){
    super(props);
    this.state = {
      notifications: []
    }
  }

  componentDidMount(){
    this.checkNotifications();
    this.interval = setInterval(() => this.checkNotifications(), 5000);
  }

  componentWillUnmount() {
    if(this.interval)
    clearInterval(this.interval);
  }

  checkNotifications = () => {
    getNotifications().then((data) => {
      let sortFunc = (a,b) => new Date(b.sentOn) - new Date(a.sentOn);
      data.sort(sortFunc);
      this.setState({notifications: data});
    });
  }

  onNotificationClick = (id, index) => {
    let data = this.state.notifications;
    data[index].isRead = true;
    this.setState({notifications: data});
    setNotificationAsRead(id);
  }

  render() {
    let count = 0;
    for(let i = 0; i < this.state.notifications.length; i++){
        if(!this.state.notifications[i].isRead){
          count++;
        }
    }

    let notificationList = this.state.notifications.map((item, index) => (
            <button className={item.isRead ? "notification-item" : "notification-item not-read"} onClick={() => this.onNotificationClick(item.id, index)}>
                  <span>{item.message}</span>
            </button>
      ));

    return (
        <div className="notification-container">
                <button className="btn btn-light" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  <img src={require("../../assets/notification.png")} />
                  {count !== 0 ? (<span class="notification">{count}</span>) : ""}
                </button>
                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton">
                    <div className="notifications-list">
                        {notificationList}
                   </div>
                </div>
                        
        </div>
        );
    }
}
