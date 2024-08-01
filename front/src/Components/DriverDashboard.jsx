import React, {useState, useEffect} from 'react';
import { FaCar } from 'react-icons/fa';
import { FaRoad } from 'react-icons/fa';
import { FaSignOutAlt } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';
import { fetchUserInfo, updateUserFields, formatDateOnly, generateImageUrl } from '../Services/UserProfileService';
import { FaSpinner, FaTimes } from 'react-icons/fa';
import {getAvailRides} from '../Services/DriverService.js'
import { DriveAccept } from '../Services/DriverService.js';
import { getCurrentRide } from '../Services/DriverService.js';
import RidesDriver from './RidesDriver.jsx';

import { FaCheckCircle } from 'react-icons/fa';
import { MdPerson } from 'react-icons/md';


const sidebarStyle = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-start',
    width: '250px',
    padding: '20px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    position: 'fixed',
    left: '0',
    top: '0',
    bottom: '0',
};

const mainContentStyle = {
    marginLeft: '270px', // Dodajemo dovoljno prostora za sidebar
    padding: '20px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    minHeight: '100vh',
    fontFamily: 'Arial, sans-serif',
};

const buttonStyle = {
    width: '100%',
    padding: '10px 15px',
    margin: '10px 0',
    backgroundColor: '#FFD700',
    color: '#ffffff',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
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
    maxWidth: '600px',
    maxHeight: '60vh',
    margin: '0 auto',
    padding: '20px',
    backgroundColor: '#ffffff',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    overflowY: 'auto',
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
    justifyContent: 'space-between',
    marginTop: '20px',
};



export default function DriverDashboard(props){
    const user = props.user;
    const apiEndpoint = process.env.REACT_APP_CHANGE_USER;
    const apiUserDetails = process.env.REACT_APP_GET_USER_DETAILS;
    const apiCurrentRide = process.env.REACT_APP_GET_CURRENT_RIDE_DRIVER;
    const apiAllRides = process.env.REACT_APP_GET_ALL_RIDES_NOT_FINISHED;
    const apiAcceptNewDrive = process.env.REACT_APP_ACCEPT_NEW_DRIVE;

    const idUser = user.id;
    localStorage.setItem("idUser", idUser);
    const jwt = localStorage.getItem('token');
    const navigate = useNavigate();

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');
    const [repeatNewPassword, setRepeatNewPassword] = useState('');
    const [birthday, setBirthday] = useState('');
    const [email, setEmail] = useState('');
    const [address, setAddress] = useState('');
    const [image, setImage] = useState(null);
    // eslint-disable-next-line no-unused-vars
    const [isEditing, setIsEditing] = useState(false);
    const [pom, setPom] = useState('editProfile');
    const [fileSelected, setFileSelected] = useState(null);
    const [userInit, setUserInit] = useState({});
    // eslint-disable-next-line no-unused-vars
    const [role, setRole] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [avgRating, setAvgRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [sumRating, setSumRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [isVerified, setIsVerified] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [isBlocked, setIsBlocked] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [status, setStatus] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [numRating, setNumRating] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [userDetails, setUserDetails] = useState('');
    const [clockSimulation, setClockSimulation] = useState('');

    const [driveIsActive, setDriveIsActive] = useState(false);
    const [rides, setRides] = useState([]);
    // eslint-disable-next-line no-unused-vars
    const [currentRides, setCurrentRides] = useState();
    // eslint-disable-next-line no-unused-vars
    const [minutesToStart, setMinutesToStart] =useState(null);
    // eslint-disable-next-line no-unused-vars
    const [minutesToEnd, setMinutesToEnd] = useState(null);
   
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
                setUsername(user.username || '');
            } catch (error) {
                console.log('Error fetching:', error.message);
            }
        };
        fetchUserDetails();
    }, [jwt, apiUserDetails, idUser]);

    const handleSaveChangeClick = async () => {
        const ChangeUserDetails = await updateUserFields(apiEndpoint, firstName, lastName, birthday, address, email, password, fileSelected, username, jwt, newPassword, repeatPassword, oldPassword, idUser)
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
        setFileSelected(null);
    }

    const handleSignOut = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    const handleEditProfile = async () => {
        try {
            setPom('editProfile');
        } catch (error) {
            console.error("Error when I try to show profile", error);
        }
    };

    const handleViewRides = async () => {
        try {
            fetchRides();
            setPom('rides');
        } catch (error) {
            console.error("Error when I try to show profile", error);
        }
    };

    const fetchRides = async () => {
        try {
            const data = await getAvailRides(jwt, apiAllRides);
            console.log("Rides:", data);
            setRides(data.rides);
            console.log("Seted rides", rides);
        } catch (error) {
            console.error('Error fetching drivers:', error);
        }
    };

    const handleAcceptNewDrive = async(idDrive)=>{
        try {
            const data = await DriveAccept(apiAcceptNewDrive, idUser, idDrive, jwt); // Drive accepted
            setCurrentRides(data.ride);
            setDriveIsActive(true);
            setMinutesToStart(data.ride.minutesToStart);
            setMinutesToEnd(data.ride.minutesToEnd);
        } catch (error) {
            console.error('Error accepting drive:', error.message);
        }
    };

    const handleEditClick = () => {
        setIsEditing(true);
    };

    const handleDriveHistory = () => {
        setPom("driveHistory");
    }
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

    useEffect(() => {
        const fetchRideData = async () => {
            try {
                const data = await getCurrentRide(jwt, apiCurrentRide, idUser);
                console.log("Active trip:", data);

                if (data.error && data.error.status === 400) {
                    setClockSimulation("You don't have an active trip!");
                    return;
                }

                if (data.drive) {
                    console.log("Active trip:", data.drive);

                    if (data.drive.accepted && data.drive.timeToDriverArrivalSeconds > 0) {
                        setClockSimulation(`You will arrive in: ${data.drive.timeToDriverArrivalSeconds} seconds`);
                    } else if (data.drive.accepted && data.drive.timeToEndTripInSeconds > 0) {
                        setClockSimulation(`The trip will end in: ${data.drive.timeToEndTripInSeconds} seconds`);
                    } else if (data.drive.accepted && data.drive.timeToDriverArrivalSeconds === 0 && data.drive.timeToEndTripInSeconds === 0) {
                        setClockSimulation("Your trip has ended");
                    }
                } else {
                    setClockSimulation("You don't have an active trip!");
                }
            } catch (error) {
                console.log("Error fetching ride data:", error);
                setClockSimulation("An error occurred while fetching the trip data.");
            }
        };


        // Fetch data immediately on mount
        fetchRideData();

        // Set up an interval to fetch data every 1 second
        const intervalId = setInterval(fetchRideData, 1000);

        // Clean up the interval when the component is unmounted
        return () => clearInterval(intervalId);
    }, [jwt, apiCurrentRide, idUser]);

    return (
        <div style={{ display: 'flex', flexDirection: 'row', height: '100vh' }}>
            <div style={sidebarStyle}>
                <button className="button-logout" onClick={handleSignOut} style={buttonStyle}>
                    <div style={{ display: 'flex', alignItems: 'center' }}>
                        <FaSignOutAlt size={25} style={{ marginRight: '4px' }} />
                        <span>Sign out</span>
                    </div>
                </button>
                <div>
                    {isBlocked ? (
                        <p style={{ color: "white", fontSize: "20px", display: "flex", alignItems: "center" }}>
                            <FaTimes style={{ marginRight: "10px" }} /> You are blocked
                        </p>
                    ) : (
                        <p style={{ color: "white", fontSize: "20px", display: "flex", alignItems: "center" }}>
                            Hi, {username}
                            <span style={{ marginLeft: "10px" }}>
                                {status === 0 && <FaSpinner />}
                                {status === 1 && <FaCheckCircle />}
                                {status === 2 && <FaTimes />}
                            </span>
                        </p>
                    )}
                </div>
                <hr style={{ width: '100%', margin: '10px 0' }} />
                {clockSimulation === "You don't have an active trip!" ? (
                    <>
                        <button className="button" onClick={handleEditProfile} style={buttonStyle}>
                            <div style={{ display: 'flex', alignItems: 'center' }}>
                                <MdPerson size={25} style={{ marginRight: '30px' }} />
                                <span>Profile</span>
                            </div>
                        </button>
                        {!isBlocked && isVerified === true ? (
                            <>
                                <button className="button" onClick={handleViewRides} style={buttonStyle}>
                                    <div style={{ display: 'flex', alignItems: 'center' }}>
                                        <FaCar size={25} style={{ marginRight: '30px' }} />
                                        <span>New rides</span>
                                    </div>
                                </button>
                                <button className="button" onClick={handleDriveHistory} style={buttonStyle}>
                                    <div style={{ display: 'flex', alignItems: 'center' }}>
                                        <FaRoad size={25} style={{ marginRight: '30px' }} />
                                        <span>Rides history</span>
                                    </div>
                                </button>
                            </>
                        ) : null}
                    </>
                ) : null}
                <p style={{ color: 'white', marginTop: '20px' }}>{clockSimulation}</p>
            </div>
            <div style={mainContentStyle}>
                {!driveIsActive ? (
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        {pom === 'editProfile' && (
                            <div style={profileContainerStyle}>
                                <h2 style={{ marginBottom: '20px' }}>Edit Profile</h2>
                                <img src={image} alt="User" style={profileImageStyle} />
                                {isEditing ? (
                                    <div>
                                        <input type='file' onChange={handleImageChange} />
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Username:</label>
                                            <input className='customView-div' type='text' value={username} onChange={(e) => setUsername(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>First Name:</label>
                                            <input className='customView-div' type='text' value={firstName} onChange={(e) => setFirstName(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Last Name:</label>
                                            <input className='customView-div' type='text' value={lastName} onChange={(e) => setLastName(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Address:</label>
                                            <input className='customView-div' type='text' value={address} onChange={(e) => setAddress(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Birthday:</label>
                                            <input className='customView-div' type='text' value={birthday} onChange={(e) => setBirthday(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Email:</label>
                                            <input className='customView-div' type='email' value={email} onChange={(e) => setEmail(e.target.value)} style={{ ...profileInputStyle, width: '250px' }} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Old Password:</label>
                                            <input className='customView-div' type='password' value={oldPassword} onChange={(e) => setOldPassword(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>New Password:</label>
                                            <input className='customView-div' type='password' value={newPassword} onChange={(e) => setNewPassword(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileSectionStyle}>
                                            <label style={labelStyle}>Repeat New Password:</label>
                                            <input className='customView-div' type='password' value={repeatNewPassword} onChange={(e) => setRepeatNewPassword(e.target.value)} style={profileInputStyle} />
                                        </div>
                                        <div style={profileButtonContainerStyle}>
                                            {!isBlocked && isVerified ? (
                                                <div>
                                                    <button className='edit-button' onClick={handleSaveChangeClick} style={editButtonStyle}>Save</button>
                                                    <button className='edit-button' onClick={handleCancelClick} style={cancelButtonStyle}>Cancel</button>
                                                </div>
                                            ) : null}
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
                                        <button className='edit-button' onClick={handleEditClick} style={editButtonStyle}>Edit</button>
                                    </div>
                                )}
                            </div>
                        )}
                        {pom === 'rides' && (
                            clockSimulation === "You don't have an active trip!" ? (
                                <div style={{ width: '100%' }}>
                                    <table style={{ width: '100%' }}>
                                        <thead>
                                            <tr>
                                                <th>Location</th>
                                                <th>Destination</th>
                                                <th>Price</th>
                                                <th>Confirmation</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {rides.map((val) => (
                                                <tr key={val.idDrive}>
                                                    <td>{val.toLocation}</td>
                                                    <td>{val.fromLocation}</td>
                                                    <td>{val.price}</td>
                                                    <td>
                                                        <button
                                                            style={{
                                                                borderRadius: '20px',
                                                                padding: '5px 10px',
                                                                color: 'white',
                                                                fontWeight: 'bold',
                                                                cursor: 'pointer',
                                                                outline: 'none',
                                                                backgroundColor: 'green'
                                                            }}
                                                            onClick={() => handleAcceptNewDrive(val.tripId)}
                                                        >
                                                            Accept
                                                        </button>
                                                    </td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            ) : null
                        )}
                        {pom === 'driveHistory' && (
                            <RidesDriver />
                        )}
                    </div>
                ) : null}
            </div>
        </div>
    );
    
    
    
    
    
}

