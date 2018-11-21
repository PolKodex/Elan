const axios = require('axios');

export const getNotifications = async () => {
    var api = "/api/Notifications/GetNotifications";

    return await axios.get(api,
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);
};

export const setNotificationAsRead = async (notificationId) => {
    var api = "/api/Notifications/MarkAsRead";

    return await axios.post(api,"\"" + notificationId + "\"", 
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
        }
        }).then((response) => response.data);

};