import * as jwtUtils from '../utils/JwtUtils';

export const userIsAuthenticated = () => {
    const token = localStorage.getItem('token');
    if (token !== null) {
        const decodedToken = jwtUtils.decodeJwt(localStorage.getItem('token'));
        const currentUnixTimestamp = Math.round((new Date()).getTime() / 1000);

        if (decodedToken.exp > currentUnixTimestamp) {
            return true;
        }
    }
    return false;
}