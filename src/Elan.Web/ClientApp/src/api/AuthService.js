const axios = require('axios');

export const signIn = async (username, password) => {
    var host = "http://localhost:59549/api/Auth/Login";

    return axios.post(host,
        {
            UserName: username,
            Password: password
        })
        .then(function (response) {
            return response.data;
        });
};