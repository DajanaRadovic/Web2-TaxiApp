import React from  'react';
import {useLocation} from 'react-router-dom';
import AdminDashboard from './AdminDashboard.jsx';
import {RiderDashboard} from './RiderDashboard.jsx';
import {DriverDashboard} from './DriverDashboard.jsx';

export default function Dashboard(){
  //const [userName, setUserName] = useState('dusan.rs');
  const location = useLocation();
  const user = location.state?.user;
  console.log("This is user from main dashboard");
  const role = user["roles"];
 // const token = localStorage.getItem('token');
  return (
   <div>
     {role === 0 && <AdminDashboard user={user}/>}
     {role === 1 && <RiderDashboard user={user}/>}
     {role === 2 && <DriverDashboard user={user}/>}
   </div>
  );
}