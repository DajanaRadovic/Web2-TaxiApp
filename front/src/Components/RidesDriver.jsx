import React, {useState, useEffect} from 'react';
import { getMyRidesDriver } from '../Services/DriverService';

export default function RidesDriver(){

    const[rides, setRides] = useState([]);
    const token = localStorage.setItem('token');
    const apiEndpoint = process.env.REACT_APP_GET_RIDES_DRIVER;

    //funkcija za dobavljanje vozaca
    const fetchDrivers = async()=>{
        try {
            const data = await getMyRidesDriver(token, apiEndpoint, localStorage.getItem('idUser'));
            console.log("Rides:", data.rides);
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
        <div className="centered" style={{ width: '100%', height: '10%' }}>
            <table className="styled-table">
                <thead>
                    <tr>
                        <th>From</th>
                        <th>To</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {rides.map((ride, index) => (
                        <tr key={index}>
                            <td>{ride.toLocation}</td>
                            <td>{ride.fromLocation}</td>
                            <td>{ride.price}&euro;</td>

                        </tr>
                    ))}
                </tbody>
            </table>

        </div>
    );
}