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

export const register = async (userName, password, email, firstName, lastName, gender, question, answer) => {
    var api = "/api/Auth/Register";

    return axios.post(api,
        {
            Email: email,
            Password: password,
            UserName: userName,
            FirstName: firstName,
            LastName: lastName,
            Question: question,
            Answer: answer,
            Gender: gender
        })
        .then ((response) => {
            return response.data;
        })
}

export const getHintQuestion = async (userName) => {
    var api = "/api/Auth/GetPasswordHintQuestion";
        return axios.get(api, {params: {
            UserName: userName
        }})
        .then((response) => {
            return response.data;
        })
}

export const changePassword = async (userName, password, answer) => {
    var api = "/api/Auth/ChangePassword";

    return axios.post(api,
        {
                UserName: userName,
                Password: password,
                Answer: answer
            })
        .then((response) => {
            return response.data;
        })
}