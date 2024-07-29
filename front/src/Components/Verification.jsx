/* eslint-disable react-hooks/exhaustive-deps */
import React, { useState, useEffect } from 'react';
import { GetDriversForVerification, VerifyDriver } from '../Services/AdminService';
import { FaCheckCircle } from 'react-icons/fa';

export default function Verification() {
    const [drivers, setDrivers] = useState([]);
    const token = localStorage.getItem('token');
    const getDriversEndpoint = process.env.REACT_APP_GET_GET_NOT_VERIFIED;
    const driversVerifyEndpoint = process.env.REACT_APP_DRIVER_VERIFY;

    const fetchDrivers = async () => {
        try {
            const data = await GetDriversForVerification(getDriversEndpoint, token);
            console.log("Fetched Data:", data);
            if (data && data.drivers && data.drivers.length > 0) {
                setDrivers(data.drivers);
            } else {
                setDrivers([]);
            }
        } catch (error) {
            console.error('Error fetching drivers:', error);
            if (error.response) {
                console.error('Error response data:', error.response.data);
                console.error('Error response status:', error.response.status);
            }
            setDrivers([]);
        }
    };

    const handleAccept = async (driver) => {
        const { id, email } = driver;
        try {
            await VerifyDriver(driversVerifyEndpoint, id, "Prihvaceno", email, token);
            fetchDrivers(); // Refresh the list after accepting a driver
        } catch (error) {
            console.error('Error accepting driver:', error);
        }
    };

    const handleDecline = async (driver) => {
        const { id, email } = driver;
        try {
            await VerifyDriver(driversVerifyEndpoint, id, "Odbijeno", email, token);
            fetchDrivers(); // Refresh the list after declining a driver
        } catch (error) {
            console.error('Error declining driver:', error);
        }
    };

    useEffect(() => {
        fetchDrivers();
    }, []); // Empty dependency array ensures this runs only once after the initial render

    return (
        <div style={{ width: '100%', padding: '20px' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ backgroundColor: '#f4f4f4' }}>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Name</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Last Name</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Email</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Username</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Verification Status</th>
                    </tr>
                </thead>
                <tbody>
    {drivers.map((driver, index) => {
        return (
            <tr key={driver.id} style={{ backgroundColor: index % 2 === 0 ? '#fff' : '#f9f9f9' }}>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{driver.firstName}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{driver.lastName}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{driver.email}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{driver.username}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                    {driver.status === 1 ? (
                        <FaCheckCircle color="green" />
                    ) : (
                        <>
                            <button
                                style={{
                                    backgroundColor: '#4CAF50',
                                    color: 'white',
                                    border: 'none',
                                    padding: '10px 20px',
                                    textAlign: 'center',
                                    textDecoration: 'none',
                                    display: 'inline-block',
                                    fontSize: '16px',
                                    margin: '4px 2px',
                                    cursor: 'pointer',
                                }}
                                onClick={() => handleAccept(driver)}
                            >
                                Accept
                            </button>
                            <button
                                style={{
                                    backgroundColor: '#f44336',
                                    color: 'white',
                                    border: 'none',
                                    padding: '10px 20px',
                                    textAlign: 'center',
                                    textDecoration: 'none',
                                    display: 'inline-block',
                                    fontSize: '16px',
                                    margin: '4px 2px',
                                    cursor: 'pointer',
                                }}
                                onClick={() => handleDecline(driver)}
                            >
                                Decline
                            </button>
                        </>
                    )}
                </td>
            </tr>
        );
    })}
</tbody>
            </table>
        </div>
    );
}
