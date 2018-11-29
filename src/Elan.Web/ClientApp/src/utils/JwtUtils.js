export const decodeJwt = (token) => {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace('-', '+').replace('_', '/');
    return JSON.parse(window.atob(base64));
};

export const getUserId = (state) => {
    if (state.userId === undefined || state.userId.trim() === "") {
        return jwtUtils.decodeJwt(localStorage.getItem('token')).jti;
    }
    return state.userId;
}
