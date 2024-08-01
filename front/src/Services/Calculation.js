import axios from "axios";
import qs from 'qs';

export async function getEstimation(jwt, apiEndpoint, fromLocation,ToLocation) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            }
        };
        const queryParams = qs.stringify({ ToLocation: ToLocation,FromLocation:fromLocation });

        const url = `${apiEndpoint}?${queryParams}`;
        const response = await axios.get(url, config);
        return response.data;
    } catch (error) {
        console.error('Error fetching data (async/await):', error.message);
        throw error;
    }
}


export async function AcceptDrive(apiEndpoint, id, jwt,fromLocation,toLocation,price,accepted,minutes) {
    try {
        
        const response = await axios.put(apiEndpoint, {
            IdRider: id,
            ToLocation: toLocation,
            FromLocation : fromLocation,
            Price : price,
            accepted : accepted,
            minutes : minutes
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

export function convertTimeStringToMinutes(timeString) {
    // Split the time string into its components
    const parts = timeString.split(':');
    
    // Extract hours, minutes, and seconds
    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    const seconds = parseInt(parts[2], 10);
    
    // Convert the time to total minutes
    const totalMinutes = (hours * 60) + minutes + (seconds / 60);
    
    return totalMinutes;
}
