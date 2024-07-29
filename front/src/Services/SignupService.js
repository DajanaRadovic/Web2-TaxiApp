import axios from 'axios';
import {SHA256} from 'crypto-js';

export function ErrorUser(firstNameError,lastNameError,usernameError,passwordError,birthdayError,emailError,addressError,imageUrlError,typeUserError){
    let formIsValid = true;
    if(firstNameError){
        alert("Name is required");
     formIsValid = false;

    }else if(lastNameError){
        alert("LastName is required");
        formIsValid = false;
    }else if(usernameError){
        alert("Username is req");
        formIsValid = false;
    }else if(passwordError){
        alert("Password is req");
        formIsValid = false;
    }else if(birthdayError){
        alert("Birth is req");
        formIsValid = false;
    }else if(emailError){
        alert("Email is req");
        formIsValid = false;
    }else if(addressError){
        alert("Address is req");
        formIsValid = false;
    }else if(imageUrlError){
        alert("Image is req");
        formIsValid = false;
    }else if(typeUserError){
        alert("Type User is req");
        formIsValid = false;
    }
    return formIsValid;
}

export async function SignupApi( firstNameError,lastNameError, usernameError,passwordError,repeatPasswordError,birthdayError,emailError,addressError,imageUrlError,typeUserError,firstName,lastName,username,password,repeatPassword,birthday, email, address, imageUrl,typeUser,signupEndpoint){
    const finished = ErrorUser(firstNameError,lastNameError,usernameError,passwordError,repeatPasswordError,birthdayError,emailError,addressError,imageUrlError,typeUserError);
    if(finished){
        const data = new FormData();
        data.append('firstName', firstName);
        data.append('lastName', lastName);
        data.append('username', username);
        data.append('password', SHA256(password).toString());
        data.append('birthday', birthday);
        data.append('email', email);
        data.append('address', address);
        data.append('imageUrl', imageUrl);
        data.append('typeUser', typeUser);

        if (SHA256(password).toString() === SHA256(repeatPassword).toString()) {
            try {
              await axios.post(signupEndpoint, data, {
                headers: {
                  'Content-Type': 'multipart/form-data'
                }
              });
              return true;
            } catch (error) {
              if (error.response && error.response.status === 409) {
                alert(`${error.response.data}\nTry another email!!!`);
              } else {
                console.error('Signup error:', error);
              }
            }
          } else {
            alert("Passwords do not match!");
          }
        }
        return false;
}