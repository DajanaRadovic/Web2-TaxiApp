import axios from "axios";
import qs from 'qs';

export async function getCurrentRide(jwt, apiEndpoint,idUser) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            }
        };
        console.log(apiEndpoint);
        const queryParams = qs.stringify({ id: idUser });

        const url = `${apiEndpoint}?${queryParams}`;
        const response = await axios.get(url, config);
        return response.data;
    } catch (error) {
     
        return { error: error.response };
    }
}

export async function getMyRides(jwt, apiEndpoint,idUser) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            }
        };
        console.log(apiEndpoint);
        const queryParams = qs.stringify({ id: idUser });

        const url = `${apiEndpoint}?${queryParams}`;
        const response = await axios.get(url, config);
        return response.data;
    } catch (error) {
       
        return { error: error.response };
    }
}

export async function getNotRated(jwt, apiEndpoint) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            }
        };
        console.log(apiEndpoint);

        const response = await axios.get(apiEndpoint, config);
        return response.data;
    } catch (error) {
        return { error: error.response };
    }
}

export async function SubmitRating(apiEndpoint,jwt,rating,idDrive) {
    try {
        
        const response = await axios.put(apiEndpoint, {
            idDrive: idDrive,
            rating: rating
        }, {
            headers: {
                Authorization: `Bearer ${jwt}`
            }
        });
        console.log("This is response",response);
        return response.data;
    } catch (error) {
        console.error('Error while calling api for login user:', error);
        return error;
    }
}


