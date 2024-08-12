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


export function formatDateOnly(dateTime) {
    const dateObj = new Date(dateTime);

    // Get the date components
    const year = dateObj.getFullYear();
    const month = dateObj.getMonth() + 1; // Meseci su 0-indeksirani, dodaj 1
    const day = dateObj.getDate();

    // Format the date as 'MM-dd-yyyy'
    return `${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}-${year}`;
}

/*
export async function updateUserFields(apiEndpoint, firstName, lastName, birthday, address, email, password, username, jwt, image, newPassword, newPasswordRepeat, oldPasswordRepeat, id) {
    console.log("Username before adding to FormData:", username);
    const formData = new FormData();
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Birthday', birthday);
    formData.append('Address', address);
    formData.append('Email', email);
    formData.append("Id", id);
    

    if (newPassword !== '') {
        const hashPw = SHA256(newPassword).toString();
        formData.append('Password', hashPw);
    } else { 
        const hashPw = '';
        formData.append('Password', hashPw);
    }
    if(image){
        formData.append('Image', image);
    }
    if(username){
        formData.append('Username', username);
    }
    console.log("FormData before sending to API:", Array.from(formData.entries()));
    let data;

    if (oldPasswordRepeat !== '' || newPasswordRepeat !== '' || newPassword !== '' && validateNewPasswords(password, oldPasswordRepeat, newPassword, newPasswordRepeat)) {
        console.log("Successfully entered new passwords");
        data = await updateUserFieldsApi(apiEndpoint, formData, jwt);
        console.log("data other call", data);
    } else if (oldPasswordRepeat === '' && newPasswordRepeat === '' && newPassword === '') {
        data = await updateUserFieldsApi(apiEndpoint, formData, jwt);
    }

    if (data && data.user) {
        console.log("Changed user data:", data.user);
        return data.user;
    } else {
        console.error("data or data.user is undefined:", data);
        return null; // or handle this case appropriately
    }
}
*/

export async function updateUserFields(apiEndpoint, firstName, lastName, birthday, address, email, password, username, jwt, image, newPassword, newPasswordRepeat, oldPasswordRepeat, id) {
    const formData = new FormData();
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Birthday', birthday);
    formData.append('Address', address);
    formData.append('Email', email);
    formData.append('Id', id);

    if (newPassword !== '') {
        const hashPw = SHA256(newPassword).toString();
        formData.append('Password', hashPw);
    } else { 
        formData.append('Password', '');
    }

    if (image) {
        formData.append('Image', image);
    }
    formData.append('Username', username);

    console.log("FormData pre slanja na API:", Array.from(formData.entries()));
    let data;

    if (oldPasswordRepeat !== '' || newPasswordRepeat !== '' || newPassword !== '' && validateNewPasswords(password, oldPasswordRepeat, newPassword, newPasswordRepeat)) {
        console.log("Uspešno unete nove lozinke");
        data = await updateUserFieldsApi(apiEndpoint, formData, jwt);
        console.log("Podaci sa drugog poziva", data);
    } else if (oldPasswordRepeat === '' && newPasswordRepeat === '' && newPassword === '') {
        data = await updateUserFieldsApi(apiEndpoint, formData, jwt);
    }

    if (data && data.user) {
        console.log("Promenjeni korisnički podaci:", data.user);
        return data.user;
    } else {
        console.error("Podaci ili data.user nisu definisani:", data);
        return null; // ili obradi ovaj slučaj na odgovarajući način
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
        console.log("FormData before API call:", Array.from(formData.entries()));
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
