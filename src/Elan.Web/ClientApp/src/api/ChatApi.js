const axios = require('axios');

export const getMessages = async (userId, skip = 0, take = 10) => {
    var api = "/api/Chat/GetMessages";

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
    return response.data;
};