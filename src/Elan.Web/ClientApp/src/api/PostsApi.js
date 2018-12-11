const axios = require('axios');

export const savePost = async (content, privacySetting) => {
    var api = "/api/Posts/CreatePost";

    return await axios.post(api,
        {
            content: content,
            privacySetting: privacySetting
        },
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);
};

export const getLatestPosts = async (skip, take) => {
    var api = "/api/Posts/GetLatestPosts";

    return await axios.get(api,
        {
            params: {
                skip: skip,
                take: take
            },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);

};

export const commentPost = async (content, basePostId) => {
    var api = "/api/Posts/CreatePostComment";

    return await axios.post(api,
        {
            content: content,
            basePostId: basePostId
        },
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);
};

export const getComments = async (postId, skip, take) => {
    var api = "/api/Posts/GetPostComments";

    return await axios.get(api,
        {
            params: {
                postId: postId,
                skip: skip,
                take: take
            },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);
};

export const setReaction = async (postId) => {
    var api = '/api/Posts/SetReaction';

    return await axios.post(api,
        {
            PostId: postId,
            Type: 1
        },
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);
};