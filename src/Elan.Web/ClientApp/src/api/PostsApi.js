const axios = require('axios');

export const savePost = async (content) => {
    var api = "/api/Posts/CreatePost";

    return await axios.post(api,
        {
            content: content
        }, 
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
        }
        }).then((response) => response.data);
};

export const getLatestPosts = async () => {
    var api = "/api/Posts/GetLatestsPosts";

    return await axios.get(api,
        {
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);

}