const axios = require('axios');

export const getUser = async (userId) => {
    var api = "/api/User/GetUserProfile?userId=" + userId;

    const response = await axios.get(api,
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        });

    return response;
};

export const getUserPictures = async (userId) => {
    var api = "/api/User/GetUserImages";

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

export const getUserPosts = async (userId, skip = 0, take = 10) => {
    var api = "/api/Posts/GetPostsForUser";

    const response = await axios.get(api,
        {
            params: {
                userId: userId,
                skip: skip,
                take: take
            },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        });
    return response;
};