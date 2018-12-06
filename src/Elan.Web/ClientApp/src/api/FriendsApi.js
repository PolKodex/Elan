const axios = require('axios');

export const getFriends = async (userId) => {
    var api = "/api/Friends/GetFriendsByUserId";

    const response = await axios.get(api,
        {
            params: {
                userId: userId
            },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        });

    return response;
};

export const inviteToFriends = async (userId) => {
    var api = '/api/Friends/SendInvitation';

    return await axios.post(api,"\"" + userId + "\"", 
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        });
}

export const removeFriend = async (userId) => {
    var api = '/api/Friends/RemoveFriend';

    return await axios.post(api, "\"" + userId + "\"",
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        });
}

export const acceptInvitation = async (userId) => {
    var api = '/api/Friends/AcceptInvitation';

    return await axios.post(api, "\"" + userId + "\"",
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        });
}

export const declineInvitation = async (userId) => {
    var api = '/api/Friends/DeclineInvitation';

    return await axios.post(api, "\"" + userId + "\"",
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        });
}

export const cancelInvitation = async (userId) => {
    var api = '/api/Friends/CancelInvitation';

    return await axios.post(api, "\"" + userId + "\"",
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        });
}