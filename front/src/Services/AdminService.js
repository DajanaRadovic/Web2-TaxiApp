import axios from 'axios';


export async function GetDrivers(apiEndpoint, jwtToken) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwtToken}`,
            }
        };

        const response = await axios.get(apiEndpoint, config);
        console.log("API Response:", response.data);
        return response.data;
        
    } catch (error) {
        console.error('Error fetching data (async/await):', error.message);
        throw error; 
    }
}

export async function GetDriversForVerification(apiEndpoint, jwtToken) {
    console.log(apiEndpoint);
    console.log(jwtToken);
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwtToken}`, // Include the JWT token
            }
        };

        return axios.get(apiEndpoint, config)
        .then(response => response.data);
        
    } catch (error) {
        console.error('Error fetching data (async/await):', error.message);
        throw error; // rethrow the error to handle it in the component
    }
}



export async function ChangeStatusDriver(apiEndpoint, id, changeStatus, jwt) {
    try {
        const response = await axios.put(apiEndpoint, {
            id: id,
            status: changeStatus
        }, {
            headers: {
                Authorization: `Bearer ${jwt}`
            }
        });
        console.log("Change of driver status sucesfully hapaned!",response);
    } catch (error) {
        console.error('Error while calling api change driver status:', error);
    }
}

export async function VerifyDriver(apiEndpoint, id, task,email, jwt) {
    try {
        const response = await axios.put(apiEndpoint, {
            Id: id,
            Task: task,
            Email : email
        }, {
            headers: {
                Authorization: `Bearer ${jwt}`
            }
        });
        console.log("Driver verification sucessfuly!",response);
    } catch (error) {
        console.error('Error while calling api VerifyDriver:', error);
    }
}

export async function getRidesForAdmin(jwt, apiEndpoint) {
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



