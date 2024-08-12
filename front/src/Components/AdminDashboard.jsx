import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { fetchUserInfo, updateUserFields, formatDateOnly, generateImageUrl } from '../Services/UserProfileService';

import { FaSignOutAlt } from 'react-icons/fa';
import Drivers from './Drivers';
import Verification from './Verification';
import RidesAdmin from './RidesAdmin';
import {MdPerson} from 'react-icons/md';


const sidebarStyle = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-start', // Poravnanje dugmadi levo
    width: '250px', // Širina sidebar-a
    padding: '20px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)', // Prozirna bela boja
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    position: 'fixed', // Da bi meni bio fiksiran na levoj strani
    left: '0', // Postavlja meni na levu stranu
    top: '0', // Postavlja meni na vrh stranice
    bottom: '0', // Da bi se protezao do dna stranice
};

const mainContentStyle = {
    marginLeft: '250px', // Povećano da bi se pomerio dalje od sidebar-a
    padding: '20px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)', // Pozadina sa prozirnošću
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)', // Blaga senka
    borderRadius: '8px', // Zaobljeni uglovi
    minHeight: '100vh', // Da bi sadržaj zauzimao celu visinu stranice
    fontFamily: 'Arial, sans-serif',
    width: 'calc(100% - 250px)',
};

const buttonStyle = {
    width: '100%',
    padding: '10px 10px',
    margin: '10px 0px',
    backgroundColor: '#FFD700', // Narandžasta boja
    color: 'black',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    fontWeight:'bold'
    
};

const buttonStyleMenu = {
    width: '100%',
    padding: '20px 20px',
    margin: '10px 0px',
    backgroundColor: '#FFD700', // Narandžasta boja
    color: 'black',
    border: 'none',
    borderRadius: '20px',
    cursor: 'pointer',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    fontWeight:'bold'
    
};



const profileImageStyle = {
    width: '100px',
    height: '100px',
    borderRadius: '50%',
    marginBottom: '20px',
};

const labelStyle = {
    fontWeight: 'bold',
    marginBottom: '5px',
};

const editButtonStyle = {
    ...buttonStyle,
    backgroundColor: '#28a745',
};

const cancelButtonStyle = {
    ...buttonStyle,
    backgroundColor: '#dc3545',
};

const profileContainerStyle = {
    maxWidth: '800px', // Maksimalna širina za formu
    width: '80%', // Postavite širinu na 100% unutar glavnog sadržaja
    maxHeight: '60vh', // Maksimalna visina za formu, prilagodi prema potrebi
    margin: '0 auto', // Centriranje forme u sredini
    padding: '20px',
    backgroundColor: '#ffffff',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)', // Blaga senka
    borderRadius: '8px', // Zaobljeni uglovi
    overflowY: 'auto', // Omogućava vertikalno skrolovanje
};

const profileSectionStyle = {
    marginBottom: '20px',
};

const profileInputStyle = {
    width: '100%',
    padding: '10px',
    border: '1px solid #ccc',
    borderRadius: '5px',
    marginBottom: '10px',
};

const profileButtonContainerStyle = {
    display: 'flex',
    justifyContent: 'space-between', // Razmak između dugmadi
    marginTop: '20px',
};

export default function AdminDashboard(props) {
    const user = props.user;
    const apiChangeUser = process.env.REACT_APP_CHANGE_USER;
    const idUser = user.id;
    const jwt = localStorage.getItem('token');
    const navigate = useNavigate();
    const apiUserDetails = process.env.REACT_APP_GET_USER_DETAILS;

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');
    const [birthday, setBirthday] = useState('');
    const [email, setEmail] = useState('');
    const [address, setAddress] = useState('');
    const [image, setImage] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [pom, setPom] = useState('verify');
    const [fileSelected, setFileSelected] = useState(null);
    const [userInit, setUserInit] = useState({});
    // eslint-disable-next-line no-unused-vars
    const [role, setRole] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [avgRating, setAvgRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [sumRating, setSumRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [isVerified, setIsVerified] = useState(false);
    // eslint-disable-next-line no-unused-vars
    const [isBlocked, setIsBlocked] = useState(false);
    // eslint-disable-next-line no-unused-vars
    const [status, setStatus] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [numRating, setNumRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [userDetails, setUserDetails] = useState('');

    useEffect(() => {
        const fetchUserDetails = async () => {
            try {
                const userDetails = await fetchUserInfo(jwt, apiUserDetails, idUser);
                const user = userDetails.user;
                console.log(user);
                setUserDetails(user);
                setUserInit(user);
                setAddress(user.address || '');
                setAvgRating(user.avgRating);
                setBirthday(formatDateOnly(user.birthday));
                setEmail(user.email);
                setFirstName(user.firstName);
                setImage(generateImageUrl(user.image));
                setIsBlocked(user.isBlocked);
                setIsVerified(user.isVerified);
                setLastName(user.lastName);
                setNumRating(user.numRating);
                setPassword(user.password);
                setRole(user.role);
                setStatus(user.status);
                setSumRating(user.sumRating);
                setUsername(user.username);
            } catch (error) {
                console.log('Error fetching:', error.message);
            }
        };
        fetchUserDetails();
    }, [jwt, apiUserDetails, idUser, pom]);

  
    const handleSaveChangeClick = async () => {

        console.log("Username before sending to API:",username);
        try {
            const ChangeUserDetails = await updateUserFields(
                apiChangeUser, firstName, lastName, birthday, address, email, password, username, jwt, fileSelected,  newPassword, repeatPassword, oldPassword, idUser
            );
    
            console.log("Change user:", ChangeUserDetails);
    
            if (ChangeUserDetails) {
                setUserInit(ChangeUserDetails);
                setUserDetails(ChangeUserDetails);
                setAddress(ChangeUserDetails.address);
                setAvgRating(ChangeUserDetails.avgRating);
                setBirthday(formatDateOnly(ChangeUserDetails.birthday));
                setEmail(ChangeUserDetails.email);
                setFirstName(ChangeUserDetails.firstName);
                setImage(generateImageUrl(ChangeUserDetails.image));
                setIsBlocked(ChangeUserDetails.isBlocked);
                setIsVerified(ChangeUserDetails.isVerified);
                setLastName(ChangeUserDetails.lastName);
                setNumRating(ChangeUserDetails.numRatings);
                setPassword(ChangeUserDetails.password);
                setRole(ChangeUserDetails.role);
                setStatus(ChangeUserDetails.status);
                setSumRating(ChangeUserDetails.sumRatings);
                setUsername(ChangeUserDetails.username);
                setOldPassword('');
                setNewPassword('');
                setRepeatPassword('');
                setIsEditing(false);
                setPom('editProfile');
                // setFileSelected(null);
            } else {
                console.error("ChangeUserDetails is undefined");
            }
        } catch (error) {
            console.error("Error updating user fields:", error);
        }
    };
    

    
    const handleSignOut = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    const handleShowDrivers = async() =>{
        try{
            setPom('drivers');
        }catch(error){
            console.error('Error fatching:', error.message);
        }
    }

    const handleShowForVerification = async()=>{
        try {
            setPom('verify');
        } catch (error) {
            console.error('Error fetching drivers:', error.message);
        }
    }

    const handleShowAllRides = async () =>{
        try{
            setPom('rides');
        }catch(error){
            console.error("Error when I try to show all rides", error);
        }
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        setFileSelected(file);
        const reader = new FileReader();
        reader.onloadend = () => {
            setImage(reader.result);
        };
        if (file) {
            reader.readAsDataURL(file);
        }
    };

    const handleEditProfile = async () => {
        try {
            setPom('editProfile');
        } catch (error) {
            console.error("Error when I try to show profile", error);
        }
    };


    const handleEditClick = () => {
        setIsEditing(true);
    };

    const handleCancelClick = () => {
        setIsEditing(false);
        setAddress(userInit.address);
        setAvgRating(userInit.avgRating);
        setBirthday(formatDateOnly(userInit.birthday));
        setEmail(userInit.email);
        setFirstName(userInit.firstName);
        setImage(generateImageUrl(userInit.image));
        setIsBlocked(userInit.isBlocked);
        setIsVerified(userInit.isVerified);
        setLastName(userInit.lastName);
        setNumRating(userInit.numRatings);
        setPassword(userInit.password);
        setRole(userInit.role);
        setStatus(userInit.status);
        setSumRating(userInit.sumRatings);
        setUsername(userInit.username);
        setOldPassword('');
        setNewPassword('');
        setRepeatPassword('');
        setFileSelected(null);
    };

    return (
        <div>
            <div style={sidebarStyle}>
         
                <button 
                    onClick={handleEditProfile} 
                    style={{
                    width: '50px',
                    height: '50px',
                    borderRadius: '50%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    border: 'none',
                    backgroundColor: 'white', 
                    cursor: 'pointer',
                    marginRight:'auto',
                 
                
            }}>
                     <MdPerson size={50} />
                     
                </button> 
              

                <button style={buttonStyleMenu} onClick={handleShowDrivers}>
                    DRIVERS
                </button>
                <button style={buttonStyleMenu} onClick={handleShowForVerification}>
                    VERIFY
                </button>
                <button style={buttonStyleMenu} onClick={handleShowAllRides}>
                    RIDES
                </button>
                <button style={buttonStyleMenu} onClick={handleSignOut}>
                    <FaSignOutAlt /> Sign Out
                </button>
            </div>
            <div style={mainContentStyle}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                    <h1>Admin Dashboard</h1>
                </div>
                {pom === 'editProfile' ? (
                    <div style={profileContainerStyle}>
                        <h2>Edit Profile</h2>
                        <img src={image} alt="User" style={profileImageStyle} />
                        {isEditing ? (
                            <div>
                                <input type='file' onChange={handleImageChange} />
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Username:</label>
                                    <input type='text' value={username} onChange={(e) => setUsername(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>First Name:</label>
                                    <input type='text' value={firstName} onChange={(e) => setFirstName(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Last Name:</label>
                                    <input type='text' value={lastName} onChange={(e) => setLastName(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Address:</label>
                                    <input type='text' value={address} onChange={(e) => setAddress(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Birthday:</label>
                                    <input type='text' value={birthday} onChange={(e) => setBirthday(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Email:</label>
                                    <input type='email' value={email} onChange={(e) => setEmail(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Old Password:</label>
                                    <input type='password' value={oldPassword} onChange={(e) => setOldPassword(e.target.value)} style={profileInputStyle} disabled />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>New Password:</label>
                                    <input type='password' value={newPassword} onChange={(e) => setNewPassword(e.target.value)} style={profileInputStyle} disabled />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Repeat New Password:</label>
                                    <input type='password' value={repeatPassword} onChange={(e) => setRepeatPassword(e.target.value)} style={profileInputStyle} disabled />
                                </div>
                                <div style={profileButtonContainerStyle}>
                                    <button style={editButtonStyle} onClick={handleSaveChangeClick}>Save Changes</button>
                                    <button style={cancelButtonStyle} onClick={handleCancelClick}>Cancel</button>
                                </div>
                            </div>
                        ) : (
                            <div>
                                <p><strong>Username:</strong> {username}</p>
                                <p><strong>First Name:</strong> {firstName}</p>
                                <p><strong>Last Name:</strong> {lastName}</p>
                                <p><strong>Address:</strong> {address}</p>
                                <p><strong>Birthday:</strong> {birthday}</p>
                                <p><strong>Email:</strong> {email}</p>
                                <button style={editButtonStyle} onClick={handleEditClick}>Edit</button>
                            </div>
                        )}
                    </div>
                ) : pom === 'drivers' ? (
                    <Drivers />
                ) : pom === 'verify' ? (
                    <Verification/>
                ) : pom === 'rides' ? (
                    <RidesAdmin/>
                ):null
                }
            </div>
        </div>
    );
    
    
}
