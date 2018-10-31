const axios = require('axios');

export const signIn = async (username, password) => {
    var api = "/api/Auth/Login";

    return axios.post(api,
        {
            UserName: username,
            Password: password
        })
        .then(function (response) {
            return response.data;
        });
};