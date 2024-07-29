import React, {useState, useEffect} from 'react';
import {getRidesForAdmin} from '../Services/AdminService';

export default function RidesAdmin(){

    const [rides, setRides] = useState([]);
    const token = localStorage.getItem('token');
    const apiEndpoint = process.env.REACT_APP_GET_COMPLETED_RIDES_FOR_ADMIN;

    const fetchDrivers = async() => {
        try {
            const data = await getRidesForAdmin(token, apiEndpoint);
            if(data && data.rides){
            console.log("Rides:", data.rides);
            setRides(data.rides);
            }else{
                console.log("No rides found or data format is incorrect");
                setRides([]);
            }
        } catch (error) {
            console.error('Error fetching drivers:', error);
        }
    };

    useEffect(()=>{
        fetchDrivers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className="centered" style={{ width: '100%', height: 'auto' }}>
            <table className="styled-table">
                <thead>
                    <tr>
                        <th>From</th>
                        <th>To</th>
                        <th>Price</th>
                        <th>Accepted by driver</th>
                        <th>Finished</th>
                    </tr>
                </thead>
                <tbody>
                    {rides.map((ride, index) => (
                        <tr key={index} className={ride.accepted ? 'accepted' : 'pending'}>
                            <td>{ride.fromLocation}</td>
                            <td>{ride.toLocation}</td>
                            <td>{ride.price}</td>
                            <td>{ride.accepted ? "Yes" : "No"}</td>
                            <td>{ride.isFinished ? "Yes" : "No"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
    
}