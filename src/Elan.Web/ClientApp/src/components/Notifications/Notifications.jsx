import React, { Component } from 'react';
import './Notifications.css';
import { getNotifications, setNotificationAsRead } from '../../api/NotificationsApi';
import * as signalR from '@aspnet/signalr';
import { Redirect } from 'react-router';


export default class Notifications extends Component {

    constructor(props) {
        super(props);
        this.state = {
            notifications: []
        }
    }

    componentDidMount() {
        this.checkNotifications();

        const options = {
            logMessageContent: true,
            logger: signalR.LogLevel.Trace,
            accessTokenFactory: () => localStorage.getItem('token')
        };

        this.connection = new signalR.HubConnectionBuilder()
            .withHubProtocol(new signalR.JsonHubProtocol())
            .withUrl("/notificationhub", options)
            .build();

        const startSignalRConnection = connection => connection.start()
            .then(() => console.info('Websocket Connection Established'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.start()
            .then(() => console.info('SignalR Connected'))
            .catch(err => console.error('SignalR Connection Error: ', err));

        this.connection.on('NotificationsCount', this.onNotification);
        this.connection.onclose(() => setTimeout(startSignalRConnection(this.connection), 5000));
    }

    onNotification = (count) => {
        this.checkNotifications();
    };

    checkNotifications = () => {
        getNotifications().then((data) => {
            let sortFunc = (a, b) => new Date(b.sentOn) - new Date(a.sentOn);
            data.sort(sortFunc);

            this.setState({ notifications: data });
        });
    }

    onNotificationClick = (id, index) => {
        let data = this.state.notifications;
        data[index].isRead = true;
        this.setState({ notifications: data });
        setNotificationAsRead(id);
        if (data[index].notificationType === 0 || data[index].notificationType === 1) {
            this.setState({ notificationRedirectTo: '/account/' + data[index].sourceId });
        }
  }

    render() {
        let count = 0;
        for (let i = 0; i < this.state.notifications.length; i++) {
            if (!this.state.notifications[i].isRead) {
                count++;
            }
        }

        let notificationList = this.state.notifications.map((item, index) => (
            <button key={item.id} className={item.isRead ? "notification-item" : "notification-item not-read"} onClick={() => this.onNotificationClick(item.id, index)}>
                <span>{item.message}</span>
            </button>
        ));

        let redirectTo = this.state.notificationRedirectTo;

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
                {redirectTo && <Redirect to={redirectTo} /> }                  
            </div>
        );
    }
}
