import React, {useState, useEffect} from "react";
import {getMyRides} from '../Services/RiderService'

const tableStyle = {
    width: '70%',
    borderCollapse: 'collapse',
    backgroundColor: '#fff',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    overflow: 'hidden',
};

const tableHeaderStyle = {
    backgroundColor: '#FFD700', // Narandžasta boja za zaglavlje
    color: '#fff',
    textAlign: 'left',
    padding: '20px',
};

const tableCellStyle = {
    padding: '10px',
    border: '1px solid #ddd',
    textAlign: 'left',
};

const alternatingRowStyle = (index) => ({
    backgroundColor: index % 2 === 0 ? '#f9f9f9' : '#fff',
});

const containerStyle = {
    padding: '20px',
    boxSizing: 'border-box',
    overflowX: 'auto',
    margin: '0',
    height: 'calc(100vh - 40px)', // Smanji visinu za padding
    width: 'calc(100vw - 320px)', // Postavi visinu na auto ako je potrebno
};


export default function RidesRider(){
    const [rides, setRides] = useState([]);
    const token = localStorage.getItem('token');
    const apiEndpoint = process.env.REACT_APP_GET_RIDES_RIDER;

    // Preuzimam sve voznje
    const fetchRides = async() => {
        try {
            const data = await getMyRides(token, apiEndpoint, localStorage.getItem('idUser'));
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
        fetchRides();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div style={containerStyle}>
            <table style={tableStyle}>
                <thead>
                    <tr style={tableHeaderStyle}>
                        <th style={tableCellStyle}>From</th>
                        <th style={tableCellStyle}>To</th>
                        <th style={tableCellStyle}>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {rides.map((ride, index) => (
                        <tr key={index} style={alternatingRowStyle(index)}>
                            <td style={tableCellStyle}>{ride.fromLocation}</td>
                            <td style={tableCellStyle}>{ride.toLocation}</td>
                            <td style={tableCellStyle}>{ride.price}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
    
    
    
}