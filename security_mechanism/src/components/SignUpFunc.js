import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import AuthServices from '../services/AuthServices'
import { TextField } from '@mui/material'

const authServices = new AuthServices();

export default function SignUpFunc() {

    const [firstname, setFirstname] = useState('');
    const [firstnameflag, setFirstnameflag] = useState(false);
    const [lastname, setLastname] = useState('');
    const [lastnameflag, setLastnameflag] = useState(false);
    const [username, setUsername] = useState('');
    const [usernameflag, setUsernameflag] = useState(false);
    const [email, setEmail] = useState('');
    const [emailflag, setEmailflag] = useState(false);
    const [password, setPassword] = useState('');
    const [passwordflag, setPasswordflag] = useState(false);
    const [confirmpassword, setConfirmpassword] = useState('');
    const [confirmpasswordflag, setConfirmpasswordflag] = useState(false);
    // const [flag, setflag] = useState(false);

    const handlefirstnameChange = (event) => {
        setFirstname(event.target.value);
    }
    const handlelastnameChange = (event) => {
        setLastname(event.target.value);
    }
    const handleusernameChange = (event) => {
        setUsername(event.target.value);
    }
    const handleemailChange = (event) => {
        setEmail(event.target.value);
    }
    const handlepasswordChange = (event) => {
        setPassword(event.target.value);
    }
    const handleconfirmpasswordChange = (event) => {
        setConfirmpassword(event.target.value);
    }

    function Validation() {
        setFirstnameflag(false);
        setLastnameflag(false);
        setUsernameflag(false);
        setEmailflag(false);
        setPasswordflag(false);
        setConfirmpasswordflag(false)
        if (firstname === '') {
            setFirstnameflag(true);
        }
        if (lastname === '') {
            setLastnameflag(true);
        }
        if (username === '') {
            setUsernameflag(true);
        }
        if (email === '') {
            setEmailflag(true);
        }
        if (password === '') {
            setPasswordflag(true);
        }
        if (confirmpassword === '') {
            setConfirmpasswordflag(true);
        }
        if (password !== confirmpassword) {
            setPasswordflag(true);
            setConfirmpasswordflag(true);
            setPassword('');
            setConfirmpassword('');
        }
    }

    let navigate = useNavigate();

    function Redirect() {
        alert("Successful");
        navigate('/verify-using-face');
    }

    const handleFormSubmit = (event) => {
        event.preventDefault();
        Validation()
        if (firstname !== '' && lastname !== '' && username !== '' && email !== '' && password !== '' && confirmpassword !== '') {
            console.log('Okay')

            let data = {
                firstName: firstname,
                lastName: lastname,
                userName: username,
                email: email,
                password: password,
                confirmPassword: confirmpassword
            }

            authServices.SignUp(data)
                .then((data) => {
                    console.log('data: ', data)
                    if (data.data.isSuccess === true) {
                        console.log("signed up");
                        // setflag(true);
                        Redirect();
                    }
                    else if (data.data.message === "Duplicate entry 'kakul1' for key 'PRIMARY'"){
                        alert('Username already exist, try entering different username');
                    }
                    else if (data.data.message === "Password and confirm password did not match."){
                        alert(data.data.message);
                    }
                })
                .catch((error) => {
                    console.log('Error: ', error);
                    alert("Try again!");
                })
        }
        else {
            alert('Fill field');
            console.log('Fill field')
        }
        // if (flag === true) {
        //     alert("Successful");
        //     navigate('/verify-using-face');
        // }
    }

    return (
        <>
            <div className="container">
                <div className="myBox">
                    <div>
                        <h2 className="title">Create Account</h2>
                    </div>
                    <form className="form-wrapper">
                        <div className="name">
                            <div className="row mb-3">
                                <div className="col">
                                    <TextField type="text"
                                        error={firstnameflag}
                                        className="form-control"
                                        label="First name"
                                        variant="outlined"
                                        name="firstname"
                                        value={firstname}
                                        onChange={handlefirstnameChange}
                                    />
                                </div>
                                <div className="col">
                                    <TextField type="text"
                                        error={lastnameflag}
                                        className="form-control"
                                        label="Last name"
                                        variant="outlined"
                                        name="lastname"
                                        value={lastname}
                                        onChange={handlelastnameChange}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="username">
                            <div className='row mb-3'>
                                <TextField type="text"
                                    error={usernameflag}
                                    className="form-control"
                                    label="Create Username"
                                    variant="outlined"
                                    name="username"
                                    value={username}
                                    onChange={handleusernameChange}
                                />
                            </div>
                        </div>
                        <div className="email">
                            <div className='row mb-3'>
                                <TextField type="email"
                                    error={emailflag}
                                    className="form-control"
                                    label="Email"
                                    variant="outlined"
                                    name="email"
                                    value={email}
                                    onChange={handleemailChange}
                                />
                            </div>
                        </div>
                        <div className="password">
                            <div className='row mb-3'>
                                <TextField type="password"
                                    error={passwordflag}
                                    className="form-control"
                                    label="Password"
                                    variant="outlined"
                                    name="password"
                                    value={password}
                                    onChange={handlepasswordChange}
                                />
                            </div>
                        </div>
                        <div className="confirmpassword">
                            <div className='row mb-3'>
                                <TextField type="password"
                                    error={confirmpasswordflag}
                                    className="form-control"
                                    label="Confirm Password"
                                    variant="outlined"
                                    name="confirmpassword"
                                    value={confirmpassword}
                                    onChange={handleconfirmpasswordChange}
                                />
                            </div>
                        </div>
                        <div>
                            <button className='button' onClick={handleFormSubmit}>next</button>
                        </div>
                    </form>
                    <p className='clickhere'>
                        Aleady have an account?
                        <Link to="/Login" style={{ color: '#FFF' }} >Login here</Link>
                    </p>
                </div>
            </div>
        </>
    )
}
