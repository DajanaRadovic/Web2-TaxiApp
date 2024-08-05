import React, {useState, useEffect} from 'react';
import {getRidesForAdmin} from '../Services/AdminService';

export default function RidesAdmin(){

    const [rides, setRides] = useState([]);
    const token = localStorage.getItem('token');
    const apiEndpoint = process.env.REACT_APP_GET_COMPLETED_RIDES_FOR_ADMIN;

    const fetchDrivers = async () => {
        try {
            const data = await getRidesForAdmin(token, apiEndpoint);
            console.log("API Response:", data); // Log ceo odgovor API-a
            if (data && data.length > 0) {
                console.log("Rides:", data);
                setRides(data); // Postavi rides direktno na data
            } else {
                console.log("No rides found or data format is incorrect");
                setRides([]);
            }
        } catch (error) {
            console.error('Error fetching rides:', error);
            setRides([]); // U slučaju greške, postavi rides na prazan niz
        }
    };

    /*
    const fetchDrivers = async()=>{
        try{
            const data = await getRidesForAdmin(token, apiEndpoint);
            console.log("Rides:", data.rides);
            setRides(data.rides);

        }catch(error){
            console.error('Error fatching:', error);

        }
    }*/

    useEffect(()=>{
        fetchDrivers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className="centered" style={{ width: '100%', height: 'auto' }}>
            <h2 style={{ textAlign: 'center', margin: '20px 0' }}>Rides</h2>
            <table className="styled-table" style={{ width: '100%', margin: '0 auto', fontSize: '1.2em' }}>
                <thead>
                    <tr style={{ 
                        backgroundColor: '#FFD700', // Narandžasta boja za zaglavlje
                        color: '#fff', 
                        textAlign: 'left', 
                        padding: '10px',
                        fontWeight: 'bold'
                    }}>
                        <th style={{ padding: '10px' }}>From</th>
                        <th style={{ padding: '10px' }}>To</th>
                        <th style={{ padding: '10px' }}>Price</th>
                        <th style={{ padding: '10px' }}>Accepted</th>
                        <th style={{ padding: '10px' }}>Finished</th>
                    </tr>
                </thead>
                <tbody>
                    {rides && rides.length > 0 ? (
                        rides.map((ride, index) => (
                            <tr key={index} style={{ 
                                backgroundColor: '#f9f9f9', // Svetlo siva pozadina za redove
                                borderBottom: '1px solid #ddd',
                                height: '10px' // Povećana visina redova
                            }}>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.fromLocation}</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.toLocation}</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.price}&euro;</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.accepted ? "Yes" : "No"}</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.isFinished ? "Yes" : "No"}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="5" style={{ textAlign: 'center', padding: '10px' }}>No rides available</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
    
}