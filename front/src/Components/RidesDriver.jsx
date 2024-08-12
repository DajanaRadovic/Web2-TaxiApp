import React, {useState, useEffect} from 'react';
import { getMyRidesDriver } from '../Services/DriverService';

export default function RidesDriver(){

    const[rides, setRides] = useState([]);
    const token = localStorage.getItem('token');
    const apiEndpoint = process.env.REACT_APP_GET_RIDES_DRIVER;

    
    const fetchDrivers = async()=>{
        try {
            const data = await getMyRidesDriver(token, apiEndpoint, localStorage.getItem('idUser'));
            //console.log("Rides:", data.rides);
            console.log("API Response:", data);
            if (data && Array.isArray(data.rides)) {
                setRides(data.rides);
            } else {
                setRides([]); // Ako nema voznji ili je odgovor neispravan
            }
        } catch (error) {
            console.error('Error fetching drivers:', error);
            setRides([]); // U slučaju greške, postavi rides na prazan niz
        }
    };

    useEffect(()=>{
        fetchDrivers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className="centered" style={{ width: '100%', height: '100%' }}>
            <h2 style={{ textAlign: 'center', margin: '20px 0' }}>History</h2>
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
                    </tr>
                </thead>
                <tbody>
                    {rides.map((ride, index) => (
                        <tr key={index} style={{ 
                            backgroundColor: '#f9f9f9', // Svetlo siva pozadina za redove
                            borderBottom: '1px solid #ddd',
                            height: '10px' // Povećana visina redova
                        }}>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.toLocation}</td>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.fromLocation}</td>
                            <td style={{ padding: '10px', border: '1px solid #ddd' }}>{ride.price}&euro;</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}