const axios = require('axios');

export const findUsers = async (query) => {
    var api = "/api/User/FindUsers";

    return await axios.get(api,
        {   
        	params: {
                query: query
            },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        }).then((response) => response.data);

}