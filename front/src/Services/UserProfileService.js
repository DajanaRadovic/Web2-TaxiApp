/* eslint-disable no-mixed-operators */
import axios from "axios";
import { SHA256 } from "crypto-js";
import qs from 'qs';

// Funkcija za kreiranje URL-a slike za prikaz na stranici
export function generateImageUrl(image) {
    console.log("Entering generateImageUrl function");

    
    if (image.file) {
        const byteCharacters = atob(image.file);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: image.typeContent });
        const url = URL.createObjectURL(blob);
        return url;
    } else {
        console.error('Invalid imageFile or missing properties:', image);
        return null; // Vraća null ako su podaci nevažeći
    }
}

// Funkcija za konverziju datuma i vremena u samo datum
export function formatDateOnly(dateTime) {
    const date = new Date(dateTime);

    // Izvlačenje komponenti datuma
    const year = date.getFullYear();
    const month = date.getMonth() + 1; // Meseci u JavaScriptu su 0-indeksirani
    const day = date.getDate();

    // Formatiranje datuma kao 'DD-MM-YYYY'
    return `${day.toString().padStart(2, '0')}-${month.toString().padStart(2, '0')}-${year}`;
}

// Funkcija za promenu korisničkih podataka
export async function updateUserFields(apiEndpoint, firstName, lastName, birthday, address, email, password, username, jwt, image, newPassword, newPasswordRepeat, oldPasswordRepeat,id) {
    const formData = new FormData();
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Birthday', birthday);
    formData.append('Address', address);
    formData.append('Email', email);
    formData.append("Id", id);

    // eslint-disable-next-line eqeqeq
    if(newPassword !='') {
        const hashPw = SHA256(newPassword).toString();
        formData.append('Password', hashPw);
    }
    else{ 
    const hashPw = '';
    formData.append('Password', hashPw);
    }
    formData.append('Image', image);
    formData.append('Username', username);

    // eslint-disable-next-line eqeqeq
    if(oldPasswordRepeat!='' || newPasswordRepeat!='' || newPassword!='' && validateNewPasswords(password,oldPasswordRepeat,newPassword,newPasswordRepeat)){
            console.log("Succesfully entered new passwords");
            const dataOtherCall = await updateUserFieldsApi(apiEndpoint,formData,jwt);
            console.log("data other call",dataOtherCall);
            return dataOtherCall.changedUser;
    }else if(oldPasswordRepeat ==='' && newPasswordRepeat ==='' && newPassword === ''){
        const data = await updateUserFieldsApi(apiEndpoint,formData,jwt);
        return data.changedUser;
    }
}

// Funkcija za dobijanje informacija o korisniku
export async function fetchUserInfo(jwt, apiEndpoint, userId) {
    try {
        const config = {
            headers: {
                Authorization: `Bearer ${jwt}`,
                'Content-Type': 'application/json'
            }
        };

        const queryParams = qs.stringify({ Id: userId });
        const url = `${apiEndpoint}?${queryParams}`;
        const response = await axios.get(url, config);
        return response.data;
    } catch (error) {
        console.error('Error fetching user info:', error.message);
        throw error;
    }
}

// API poziv za promenu korisničkih podataka
export async function updateUserFieldsApi(apiEndpoint, formData, jwt) {
    try {
        const response = await axios.put(apiEndpoint, formData, {
            headers: {
                'Authorization': `Bearer ${jwt}`,
                'Content-Type': 'multipart/form-data'
            }
        });
        console.log("API response data:", response.data);
        return response.data;
    } catch (error) {
        if (error.response) {
            // Server je odgovorio sa status kodom koji pada izvan opsega 2xx
            console.error('Error response data:', error.response.data);
            console.error('Error response status:', error.response.status);
            console.error('Error response headers:', error.response.headers);
        } else if (error.request) {
            // Zahtev je napravljen, ali odgovor nije dobijen
            console.error('Error request data:', error.request);
        } else {
            // Došlo je do greške prilikom podešavanja zahteva
            console.error('Error message:', error.message);
        }
        throw error; // Ponovo baci grešku
    }
}

// Funkcija za validaciju novih lozinki
export function validateNewPasswords(oldPassword, oldPasswordRepeat, newPassword, newPasswordConfirm) {
    const hashedOldPassword = SHA256(oldPasswordRepeat).toString();
    if (oldPassword !== hashedOldPassword) {
        alert("The old password you entered is incorrect");
        return false;
    }
    const hashedNewPassword = SHA256(newPassword).toString();
    const hashedNewPasswordConfirm = SHA256(newPasswordConfirm).toString();
    if (hashedNewPassword === hashedNewPasswordConfirm) return true;
    else {
        alert("New passwords do not match");
        return false;
    }
}
