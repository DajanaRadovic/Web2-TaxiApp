import React from 'react';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import { LoginApi } from '../Services/LoginService.js';
import {useNavigate} from "react-router-dom";


// Stilovi za komponentu
const containerStyle = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '100vh',
    backgroundImage: 'url("t.jpg")',
    backgroundSize: 'cover',
    backgroundPosition: 'center center',
  };
  
  const overlayStyle = {
    position: 'absolute',
    top: '0',
    left: '0',
    width: '100%',
    height: '100%',
    pointerEvents: 'none',
  };
  
  const boxStyle = {
    border: '2px solid #ccc',
    padding: '30px',
    borderRadius: '10px',
    background: 'rgba(255, 255, 255, 0.9)',
    boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)',
    width: '300px',
    textAlign: 'center',
  };
  
  const inputStyle = {
    width: '100%',
    padding: '10px',
    marginBottom: '20px',
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

  const titleStyle = {
    position: 'absolute',
    top: '20px',
    left: '20px',
    fontSize: '40px',
    color: 'yellow',
    fontWeight: 'bold',
};
  
  export default function Login() {
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState(true);
    const [password, setPassword] = useState('');
    const [passwordError, setPasswordError] = useState(true);
  
    const endpoint = process.env.REACT_APP_LOGIN;
    const navigate = useNavigate();
  
    const handleLoginClick = (e) => {
      e.preventDefault();
  
      if (emailError) {
        alert("Please type email in correct form!");
      } else if (passwordError) {
        alert("Password needs to have 8 characters, one capital letter, one number, and one special character");
      } else {
        LoginApi(email, password, endpoint)
          .then((responseOfLogin) => {
            console.log("Response from login user", responseOfLogin);
            if ("Login successful" === responseOfLogin.message) {
              localStorage.setItem('token', responseOfLogin.token);
              navigate("/Dashboard", { state: { user: responseOfLogin.user } });
            }
          })
          .catch((error) => {
            console.error("Error in login:", error);
            alert('Invalid password or email try again!');
          });
      }
    };
  
    const handlePasswordChange = (e) => {
      const value = e.target.value;
      setPassword(value);
      const isValidPassword = /^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.{8,})/.test(value);
      setPasswordError(!isValidPassword);
    };
  
    const handleEmailChange = (e) => {
      const value = e.target.value;
      setEmail(value);
      const isValidEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
      setEmailError(!isValidEmail);
    };
  
    return (
      <div style={containerStyle}>
        <div style={overlayStyle}></div>
        <div style={boxStyle}>
          <h2>Login</h2>
          <form onSubmit={handleLoginClick} method='post'>
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={handleEmailChange}
              style={inputStyle}
            />
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={handlePasswordChange}
              style={inputStyle}
              title="Password needs to have 8 characters, one capital letter, one number, and one special character"
            />
            <button type="submit" style={buttonStyle}>Login</button>
          </form>
          <p style={{ fontSize: '14px', marginTop: '10px' }}>
            Don't have an account? <Link to="/register" style={{ color: '#4CAF50' }}>Register here</Link>
          </p>
        </div>
        <div style={titleStyle}>Welcome to our <p>Taxi App</p></div>
      </div>
    );
  }


