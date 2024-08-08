import React, {useState, useEffect} from 'react';
import { getNotRated } from '../Services/RiderService';
import {FaStar} from 'react-icons/fa';
import { SubmitRating } from '../Services/RiderService';

export default function Rating(){
    const [rides, setRides] = useState([]);
    const [idDriveSelected, setIdDriveSelected] = useState(null);
    const [ratingSelected, setRatingSelected] = useState(0);
    const token = localStorage.getItem('token');

    const apiEndpoint = process.env.REACT_APP_GET_NOT_RATED;
    const ratingEndpoint = process.env.REACT_APP_SUBMIT_RATING;

    const fetchDrivers = async()=>{
        try{
            const data = await getNotRated(token, apiEndpoint);
            console.log('Rides:', data.rides);
            setRides(data.rides);
        }catch(error){
            console.error('Error fatching:', error);
        }
    };

    useEffect(()=>{
        fetchDrivers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const handleRate = (idDrive, rating)=>{
        setIdDriveSelected(idDrive);
        setRatingSelected(rating);
    };

    const submitRate = async()=>{
        if(idDriveSelected && ratingSelected){
            try{
                console.log("Provera");
                console.log(idDriveSelected);
                console.log(ratingSelected);

                const data = await SubmitRating(ratingEndpoint, token, ratingSelected, idDriveSelected);
                console.log(data);
                fetchDrivers();

            }catch(error){
                console.error('Error submitting:', error);
            }
        }
    };

    return (
        <div className="centered" style={{ width: '100%', height: 'auto', padding: '20px' }}>
            
            <table className="styled-table" style={{ width: '90%', margin: '0 auto', fontSize: '1.2em', borderCollapse: 'collapse', backgroundColor: '#fff', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)', borderRadius: '8px' }}>
                <thead>
                    <tr style={{ backgroundColor: '#FFD700', color: '#fff', textAlign: 'left', padding: '10px', fontWeight: 'bold' }}>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>From</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>To</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Price</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Rating</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {rides.map((ride, index) => (
                        <tr key={index} style={{ backgroundColor: '#f9f9f9', borderBottom: '1px solid #ddd' }}>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.fromLocation}</td>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.toLocation}</td>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.price}&euro;</td>
                            <td style={{ textAlign: 'center', padding: '10px', border: '1px solid #ddd' }}>
                                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                    {[...Array(5)].map((_, i) => (
                                        <FaStar
                                            key={i}
                                            size={20}
                                            color={i < (idDriveSelected === ride.idDrive ? ratingSelected : 0) ? 'gold' : 'grey'}
                                            onClick={() => handleRate(ride.idDrive, i + 1)}
                                            style={{ cursor: 'pointer' }}
                                        />
                                    ))}
                                </div>
                            </td>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                                {idDriveSelected === ride.idDrive && (
                                    <button
                                        onClick={submitRate}
                                        style={{
                                            borderRadius: '0px',
                                            padding: '5px 10px',
                                            color: 'white',
                                            fontWeight: 'bold',
                                            cursor: 'pointer',
                                            outline: 'none',
                                            background: 'green'
                                        }}
                                    >
                                        Submit
                                    </button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );


}