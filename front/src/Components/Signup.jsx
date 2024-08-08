import React, {useEffect, useState} from 'react';
import {GoogleLogin} from 'react-google-login';
import {gapi} from 'gapi-script';
import {Link} from 'react-router-dom';
import {SignupApi} from '../Services/SignupService.js';
import { useNavigate } from 'react-router-dom';


const containerStyle = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '100vh',
    backgroundImage: 'url("background.jpg")',
    backgroundSize: 'cover',
    backgroundPosition: 'center',
  };
  
  const formStyle = {
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)',
    width: '350px',
    maxHeight: '80vh',
    overflowY: 'auto',
  };
  
  const inputStyle = {
    width: '95%',
    padding: '10px',
    marginBottom: '10px',
    fontSize: '16px',
    border: '1px solid #ccc',
    borderRadius: '5px',
  };
  
  const buttonStyle = {
    width: '100%',
    padding: '12px',
    fontSize: '16px',
    backgroundColor: '#4CAF50',
    color: 'white',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
  };




export default function Signup(){
    const clientId = process.env.REACT_APP_CLIENT_ID;
    const signupEndpoint = process.env.REACT_APP_SIGNUP;

    const [firstName, setFirstName] = useState('');
    const [firstNameError, setFirstNameError] = useState(true);

    const [lastName, setLastName] = useState('');
    const [lastNameError, setLastNameError] = useState(true);

    const [username, setUsername] = useState('');
    const [usernameError, setUsernameError] = useState(true);

    const [password, setPassword] = useState('');
    const [passwordError, setPasswordError] = useState(true);

    const [repeatPassword, setRepeatPassword] = useState('');
    const [repeatPasswordError, setRepeatPasswordError] = useState(true);

    const [birthday, setBirthday] = useState('');
    const [birthdayError, setBirthdayError] = useState(true);

    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState(true);

    const [address, setAddress] = useState('');
    const [addressError, setAddressError] = useState(true);

    const [imageUrl, setImageUrl] = useState(null);
    const [imageUrlError, setImageUrlError] = useState(true);

    const [typeUser, setTypeUser] = useState('Driver');
    const [typeUserError, setTypeUserError] = useState(false);

    //const [googleSignup, setGoogleSignup] = useState('');
    const navigate = useNavigate();

    const handleSignupClick = async (e)=>{
        e.preventDefault();

        const resultSignup = await SignupApi(
            firstNameError,
            lastNameError,
            usernameError,
            passwordError,
            repeatPasswordError,
            birthdayError,
            emailError,
            addressError,
            imageUrlError,
            typeUserError,
            firstName,
            lastName,
            username,
            password,
            repeatPassword,
            birthday,
            email,
            address,
            imageUrl,
            typeUser,
            signupEndpoint
            
        );
        if(resultSignup){
            alert("Successfully");
            navigate("/");

        }
    };

    const handleInputChange = (setter, errorSetter) => (e) => {
        const value = e.target.value;
        setter(value);
        errorSetter(value.trim() === '');
    };

    const handleEmailChange = (e) => {
        const value = e.target.value;
        setEmail(value);
        const isValidEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
        setEmailError(!isValidEmail);
    };

    const handlePasswordChange = (e) => {
        const value = e.target.value;
        setPassword(value);
        const isValidPassword = /^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.{8,})/.test(value);
        setPasswordError(!isValidPassword);
    };

    const handleRepeatPasswordChange = (e) => {
        const value = e.target.value;
        setRepeatPassword(value);
        setRepeatPasswordError(value !== password);
    };

    const handleImageUrlChange = (e) => {
        const selectedFile = e.target.files[0];
        setImageUrl(selectedFile || null);
        setImageUrlError(!selectedFile);
    };

    const handleTypeUserChange = (e) => {
        const value = e.target.value;
        setTypeUser(value);
        if (value.trim() === '') {
            setTypeUserError(true);
        } else {
            setTypeUserError(false);
        }
    };
    useEffect(() => {
        if (clientId) {
            function start() {
                gapi.client.init({
                  clientId: clientId,
                    scope: ""
                });
            }
            gapi.load('client:auth2', start);
        } else {
            console.error("Client ID is not defined in .env file");
        }
    }, [clientId]);


    const onSuccess = (res)=>{
        const userProfile = res.profileObj;
        setEmail(userProfile.email);
        setFirstName(userProfile.givenName);
        setLastName(userProfile.familyName);

        setEmailError(!userProfile.email);
        setFirstNameError(!userProfile.givenName);
        setLastNameError(!userProfile.familyName);

        alert("Yavrsi druga polja");
    }
        

    const onFailure = (res)=>{
        console.log("Filed:", res);
    }

    return (
        <div style={containerStyle}>
          <div style={formStyle}>
            <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Register</h2>
            <form onSubmit={handleSignupClick} encType="multipart/form-data" method='post'>
              <input
                type="text"
                placeholder="First Name"
                style={inputStyle}
                value={firstName || ''}
                onChange={handleInputChange(setFirstName, setFirstNameError)}
                required
              />
              <input
                type="text"
                placeholder="Last Name"
                style={inputStyle}
                value={lastName || ''}
                onChange={handleInputChange(setLastName, setLastNameError)}
                required
              />
              <input
                type="text"
                placeholder="Username"
                style={inputStyle}
                value={username || ''}
                onChange={handleInputChange(setUsername, setUsernameError)}
                required
              />
              <input
                type="email"
                placeholder="Email"
                style={inputStyle}
                value={email || ''}
                onChange={handleEmailChange}
                required
              />
              <input
                type="password"
                placeholder="Password"
                style={inputStyle}
                value={password || ''}
                onChange={handlePasswordChange}
                required
              />
              <input
                type="password"
                placeholder="Repeat Password"
                style={inputStyle}
                value={repeatPassword || ''}
                onChange={handleRepeatPasswordChange}
                required
              />
              <input
                type="date"
                placeholder="Date of Birth"
                style={inputStyle}
                value={birthday || ''}
                onChange={handleInputChange(setBirthday, setBirthdayError)}
                required
              />
              <input
                type="text"
                placeholder="Address"
                style={inputStyle}
                value={address || ''}
                onChange={handleInputChange(setAddress, setAddressError)}
                required
              />
              <select
                value={typeUser}
                onChange={handleTypeUserChange}
                style={inputStyle}
              >
                <option value="Driver">Driver</option>
                <option value="Rider">Rider</option>
                <option value="Admin">Admin</option>
              </select>
              <input
                type="file"
                accept="image/*"
                onChange={handleImageUrlChange}
                style={{ marginBottom: '10px' }}
              />
              <button type="submit" style={buttonStyle}>Register</button>
            </form>
            <div style={{ textAlign: 'center', marginTop: '10px' }}>
              <GoogleLogin
                clientId={clientId}
                buttonText="Login with Google"
                onSuccess={onSuccess}
                onFailure={onFailure}
                cookiePolicy={'single_host_origin'}
                style={{ ...buttonStyle, backgroundColor: '#4285F4' }}
              />
              <br/>
               <Link to="/" className='link-underline'>Login now</Link>
            </div>
          </div>
        </div>
      );
}