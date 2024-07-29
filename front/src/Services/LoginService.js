import axios from 'axios';
import {SHA256} from 'crypto-js';

export async function LoginApi(email, password, endpoint){
    try {
        const response = await axios.post(endpoint, {
            email: email,
            password: SHA256(password).toString()
        });
        return response.data;
    } catch (error) {
        console.error('Error:', error);
    }
}