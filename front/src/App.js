import Dashboard from './Components/Dashboard';
import Login from './Components/Login';
import Signup from './Components/Signup';
import 'process/browser'; // or require('process/browser');

import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";

const containerStyle = {
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  minHeight: '100vh',
  backgroundImage: 'url(/t.jpg)', // Putanja od korena public foldera
  backgroundSize: 'cover',
  backgroundPosition: 'center center',
};


function App() {
  return (
    <div style={containerStyle}>
        <Router>
            <Routes>
                <Route  path="/" element={<Login />} />
                <Route  path="/Register" element={<Signup />} />
                <Route  exact path='/Dashboard' element={<Dashboard/>} ></Route>
            </Routes>
        </Router>
    </div>
  );
}
export default App;

