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