import React, {useState, useEffect} from 'react';

import { FaSignOutAlt } from 'react-icons/fa';
import { useNavigate } from "react-router-dom";
import { fetchUserInfo, updateUserFields, formatDateOnly, generateImageUrl } from '../Services/UserProfileService';
import { getEstimation, AcceptDrive, convertTimeStringToMinutes } from '../Services/Calculation.js';
import { getCurrentRide } from '../Services/RiderService.js';
import RidesRider from './RidesRider.jsx';
import Rating from './Rating.jsx';




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
    marginLeft: '150px', // Da bi se glavni sadržaj pomerio udesno i bio pored sidebar-a
    padding: '20px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    minHeight: '100vh', // Da bi sadržaj zauzimao celu visinu stranice
    fontFamily: 'Arial, sans-serif',
};

const buttonStyle = {
    width: '100%',
    padding: '10px 15px',
    margin: '10px 0',
    backgroundColor: '#FFD700', // Narandžasta boja
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
    maxWidth: '800px', 
    maxHeight: '70vh', 
    margin: '50px auto', 
    padding: '30px',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
  
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

const profileButtonContainerStyle = {
    display: 'flex',
    justifyContent: 'space-between', // Razmak između dugmadi
    marginTop: '20px',
};


export default function RiderDashboard(props){
    const user = props.user;
    const jwt = localStorage.getItem('token');
    const navigate = useNavigate();
    const apiEndpoint = process.env.REACT_APP_CHANGE_USER;
    const apiUserInfo = process.env.REACT_APP_GET_USER_DETAILS;
    const apiCalculation = process.env.REACT_APP_GET_PRICE;
    const apiForAcceptDrive = process.env.REACT_APP_PUT_ACCEPT_GIVEN_DRIVE;
    const apiForCurrentDrive = process.env.REACT_APP_GET_CURRENT_DRIVE;

    const [toLocation, setToLocation] = useState('');
    const [fromLocation, setFromLocation] = useState('');
    const [calculation, setCalculation] = useState(''); // price of drive 
    // eslint-disable-next-line no-unused-vars
    const [accepted, setAccepted] = useState(false);
    // eslint-disable-next-line no-unused-vars
    const [estimatedTime, setEstimatedTime] = useState('');
    // eslint-disable-next-line no-unused-vars
    const [tripTicketSubmited, setTripTicketSubmited] = useState(false);
    const [driversSeconds, setDriversSeconds] = useState('');
    const idUser = user.id; // user id 
    localStorage.setItem("idUser", idUser);


    const handleEstimationSubmit = async () => {
        try {
            if (toLocation === '' || fromLocation === '') alert("Please complete form!");
            else {
                const data = await getEstimation(localStorage.getItem('token'), apiCalculation, fromLocation, toLocation);
                console.log("This is estimated price and time:", data);

                const roundedPrice = parseFloat(data.price.calculatePrice).toFixed(2);
                console.log(typeof driversSeconds);
                setDriversSeconds(convertTimeStringToMinutes(data.price.driversSeconds));
                setCalculation(roundedPrice);

            }

        } catch (error) {
            console.error("Error when I try to show profile", error);
        }
    };

    const handleAcceptDriveSubmit = async () => {
        try {
            const data = await AcceptDrive(apiForAcceptDrive, idUser, jwt, fromLocation, toLocation, calculation, accepted, driversSeconds);

            if (data.message && data.message === "Request failed with status code 400") {
                alert("You have already submited tiket!");
                setDriversSeconds('');
                setCalculation('');
                setToLocation('');
                setFromLocation('');
            }
        } catch (error) {
            console.error("Error when I try to show profile", error);
        }
    };


    const handleLocationChange = (event) => {
        setFromLocation(event.target.value);
    };

    const handleDestinationChange = (event) => {
        setToLocation(event.target.value);
    };

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

    const handleSignOut = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

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
    // eslint-disable-next-line no-unused-vars
    const handleEditClick = () => {
        setIsEditing(true);
    };

    const handleNewDriveClick = () => {
        setPom('newDrive');

    }

    const handleEditProfile = () => {
        setPom('editProfile');
    }

    // eslint-disable-next-line no-unused-vars
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

    const handleDriveHistory = () => {
        setPom('history');
    };

    useEffect(() => {
        const fetchUserDetails = async () => {
            try {
                const userDetails = await fetchUserInfo(jwt, apiUserInfo, idUser);
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
    }, [jwt, apiUserInfo, idUser]);

    // eslint-disable-next-line no-unused-vars
    const handleRate = () =>{
        setPom('rate');
    }

    useEffect(() => {
        const fetchRideData = async () => {
            try {
                const data = await getCurrentRide(jwt, apiForCurrentDrive, idUser);
                console.log("Active trip:", data);

                if (data.error && data.error.status === 400) {
                    setClockSimulation("No active trip at the moment");
                    return;
                }

                if (data.drive) {
                    console.log("Active trip:", data.drive);

                    if (!data.drive.accepted) {
                        setClockSimulation("No driver has accepted your current ticket");
                    } else if (data.drive.accepted && data.drive.timeToDriverArrivalSeconds > 0) {
                        setClockSimulation(`The driver will arrive in: ${data.drive.timeToDriverArrivalSeconds} seconds`);
                    } else if (data.drive.accepted && data.drive.timeToEndTripInSeconds > 0) {
                        setClockSimulation(`The trip will end in: ${data.drive.timeToEndTripInSeconds} seconds`);
                    } else if (data.drive.accepted && data.drive.timeToDriverArrivalSeconds === 0 && data.drive.timeToEndTripInSeconds === 0) {
                        setClockSimulation("Your trip has ended");
                    }
                } else {
                    setClockSimulation("No active trip at the moment");
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
    }, [jwt, apiForCurrentDrive, idUser]);

    return (
        <div style={{ display: 'flex' }}>
            <div style={sidebarStyle}>
                <img src={image} alt="Profile" style={profileImageStyle} />
                <button style={buttonStyleMenu} onClick={handleEditProfile}>
                  PROFILE
                </button>
                <button style={buttonStyleMenu} onClick={handleNewDriveClick}>
                     NEW DRIVE
                </button>
                <button style={buttonStyleMenu} onClick={handleRate}>
                     RATE
                </button>
                <button style={buttonStyleMenu} onClick={handleDriveHistory}>
                     HISTORY
                </button>
                <button style={buttonStyleMenu} onClick={handleSignOut}>
                    <FaSignOutAlt /> Sign Out
                </button>
                <p style={{
    color: '#000', // Crna boja
    marginTop: '20px',
    fontSize: '20px', // Veći font
    fontFamily: 'Georgia, serif', // Elegantni serif font
    fontWeight: 'bold', // Bold font
    textAlign: 'center', // Poravnanje na sredinu
    padding: '15px', // Dodaj padding
    backgroundColor: '#f0f8ff', // Svetlo plava pozadina
    border: '1px solid #dcdcdc', // Siv border
    borderRadius: '10px', // Veći zaobljeni uglovi
    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)', // Delikatna senka
}}>
    {clockSimulation}
</p>
            </div>
            <div style={mainContentStyle}>
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
                                    <input type='password' value={oldPassword} onChange={(e) => setOldPassword(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>New Password:</label>
                                    <input type='password' value={newPassword} onChange={(e) => setNewPassword(e.target.value)} style={profileInputStyle} />
                                </div>
                                <div style={profileSectionStyle}>
                                    <label style={labelStyle}>Repeat New Password:</label>
                                    <input type='password' value={repeatPassword} onChange={(e) => setRepeatPassword(e.target.value)} style={profileInputStyle} />
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
                    </div>): 
                pom === 'newDrive' && (
                    <div style={profileContainerStyle}>
                        <h2>New Drive</h2>
                        <div style={profileSectionStyle}>
                            <label style={labelStyle}>From Location:</label>
                            <input
                                type="text"
                                value={fromLocation}
                                onChange={handleLocationChange}
                                style={profileInputStyle}
                            />
                        </div>
                        <div style={profileSectionStyle}>
                            <label style={labelStyle}>To Location:</label>
                            <input
                                type="text"
                                value={toLocation}
                                onChange={handleDestinationChange}
                                style={profileInputStyle}
                            />
                        </div>
                        <button style={buttonStyle} onClick={handleEstimationSubmit}>
                            Get Estimation
                        </button>
                        {calculation && (
                            <div style={profileSectionStyle}>
                                <h3>Estimation:</h3>
                                <p>Price: ${calculation}</p>
                                <p>Time: {driversSeconds} minutes</p>
                            </div>
                        )}
                        <button style={buttonStyle} onClick={handleAcceptDriveSubmit}>
                            Accept Drive
                        </button>
                    </div>
                )}
                {pom === 'rate' && (
                    <div style={profileContainerStyle}>
                        <h2 style={{ textAlign: 'center', margin: '20px 0' }}>Rate Driver</h2>
                        <Rating/>
                    </div>
                )}
                {pom === 'history' && (
                    <div style={profileContainerStyle}>
                        <h2 style={{ textAlign: 'center', margin: '20px 0' }}>History</h2>
                        <RidesRider />
                    </div>
                )}
               
            </div>
        </div>
    );
}