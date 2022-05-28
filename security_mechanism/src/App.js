import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Login from './components/Login';
import Home from './components/Home';
import RegisterUsingFace from './components/RegisterUsingFace';
import Navbar from './components/Navbar';
import SignUpFunc from './components/SignUpFunc';
import Welcome from './components/Welcome';

function App() {
    return (
        <>
            <div>
                <BrowserRouter>
                    <Navbar />
                    <Routes>
                        <Route exact path="/" element={<Home />} />
                        <Route exact path="/signup" element={<SignUpFunc />} />
                        <Route exact path="/Login" element={<Login />} />
                        <Route exact path="/verify-using-face" element={<RegisterUsingFace />} />
                        <Route exact path="/welcome" element={<Welcome />} />
                    </Routes>
                </BrowserRouter>
            </div>
        </>
    );
}

export default App;
