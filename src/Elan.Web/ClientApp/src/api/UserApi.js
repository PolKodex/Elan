const axios = require('axios');
const baseUrl = '/api/User/';

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
    };

export const uploadImage = async (imageBase64, isMain) => {
    var api = baseUrl + 'UploadImage';

    return axios.post(api,
        {
            ImageBase64: imageBase64,
            IsMain: isMain,
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        })
        .then(function (response) {
            return response.data;
        });
};

export const updateImage = async (imageBase64, isMain) => {
    var api = baseUrl + 'UpdateImage';

    return axios.put(api,
        {
            ImageBase64: imageBase64,
            IsMain: isMain,
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        })
        .then(function (response) {
            return response.data;
        });
};

export const deleteImage = async (id) => {
    var api = baseUrl + 'DeleteImage';

    return axios.delete(api,
        {
            imageId: id,
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            }
        })
        .then(function (response) {
            return response.data;
        });
};