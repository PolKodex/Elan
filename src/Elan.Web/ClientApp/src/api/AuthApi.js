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

export const register = async (userName, password, email, firstName, lastName) => {
    var api = "/api/Auth/Register";

    return axios.post(api,
        {
            Email: email,
            Password: password,
            UserName: userName
        })
        .then ((response) => {
            return response.data;
        })
}