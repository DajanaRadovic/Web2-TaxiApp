/* eslint-disable react-hooks/exhaustive-deps */
import React, {useState, useEffect} from 'react';
import { ChangeStatusDriver, GetDrivers } from '../Services/AdminService';

export default function Drivers(){
    const [drivers, setDrivers] = useState([]);
    const token = localStorage.getItem('token');
    const apiEndpoint = process.env.REACT_APP_CHANGE_STATUS_DRIVER;
    const endpointDrivers = process.env.REACT_APP_GET_DRIVERS;


    const fetchDrivers = async () => {
        try {
            const data = await GetDrivers(endpointDrivers, token);
            console.log("Fetched Data:", data); 
            if (data && data.user && data.user.length > 0) {
                setDrivers(data.user); // Postavite podatke o vozačima iz `user` svojstva
            } else {
                setDrivers([]); // Ako nema vozača, postavite prazan niz
            }
        } catch (error) {
            console.error('Error fetching drivers:', error);
            setDrivers([]);
        }
    };
   

    useEffect(() => {
        console.log("Calling fetchDrivers");
        fetchDrivers();
    }, []);

    const handleChangeStatus = async (id, isBlocked) => {
        try {
            await ChangeStatusDriver(apiEndpoint, id, !isBlocked, token); // Toggle the isBlocked value
            await fetchDrivers(); // Refresh list
        } catch (error) {
            console.error('Error changing driver status:', error);
        }
    };

    return (
        <div style={{ width: '100%', height: '100%', padding: '20px' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ backgroundColor: '#f4f4f4' }}>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Name</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Last Name</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Email</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Username</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Average Rating</th>
                        <th style={{ padding: '10px', border: '1px solid #ddd' }}>Status</th>
                    </tr>
                </thead>
                <tbody>
    {drivers.map((val, key) => {
        console.log("Driver data:", val);
        return (
            <tr key={val.id} style={{ backgroundColor: key % 2 === 0 ? '#fff' : '#f9f9f9' }}>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{val.name}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{val.lastName}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{val.email}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{val.username}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{val.avgRating}</td>
                <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                    {val.isBlocked ? (
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
                            onClick={() => handleChangeStatus(val.id, val.isBlocked)}
                        >
                            Unblock
                        </button>
                    ) : (
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
                            onClick={() => handleChangeStatus(val.id, val.isBlocked)}
                        >
                            Block
                        </button>
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