import Webcam from 'react-webcam';
import React, { useState, useRef, useCallback } from 'react'
import { TextField } from '@mui/material';
import AuthServices from '../services/AuthServices';
import { useNavigate } from 'react-router';


const authServices = new AuthServices();

export default function RegisterUsingFace() {

    const videoConstraints = {
        width: 220,
        height: 200,
        facingMode: "user"
    };
    const [image, setImage] = useState('');
    const webcamRef = useRef(null);
    // const [values, setValues] = useState({
    //     username: "",
    //     usernameflag: false
    // })
    const [username, setUsername] = useState('');
    const [usernameflag, setUsernameflag] = useState(false);
    // const [flag, setFlag] = useState(false);


    const capture = useCallback(() => {
        const imageSrc = webcamRef.current.getScreenshot();
        setImage(imageSrc)
    });

    const handleChange = (event) => {
        setUsername(event.target.value);
    }

    let navigate = useNavigate();

    function Redirect() {
        alert("Successfully uploaded your Image.");
        navigate('/login');
    }

    const handleFormSubmit = (load) => {
        load.preventDefault();
        setUsernameflag(false);
        if (username === '') {
            // setValues({ usernameflag: true })
            setUsernameflag(true);
        }
        if (username !== '' && image !== '') {
            console.log('Okay')

            let formData = new FormData();
            formData.append("file", image);
            formData.append("username", username);

            authServices.Register(formData)
                .then((formData) => {
                    console.log('data: ', formData);
                    console.log("registered");
                    Redirect();
                    // setFlag(true);
                })
                .catch((error) => {
                    console.log('Error: ', error)
                })

        }
        else {
            console.log('Fill field')
        }
        // if (flag === true) {
        //     alert("Successfully uploaded your Image.");
        //     navigate('/login');
        // }
    }


    return (
        // <div className="webcam-container">
        //     <div className="webcam-img">

        //         {image === '' ? <Webcam
        //             audio={false}
        //             height={200}
        //             ref={webcamRef}
        //             screenshotFormat="image/jpeg"
        //             width={220}
        //             videoConstraints={videoConstraints}
        //         /> : <img src={image} />}
        //     </div>
        //     <div>
        //         {image !== '' ?
        //             <button onClick={(e) => {
        //                 e.preventDefault();
        //                 setImage('')
        //             }}
        //                 className="btn">Retake Image</button> :
        //             <button onClick={(e) => {
        //                 e.preventDefault();
        //                 capture();
        //             }}
        //                 className="btn">Capture</button>
        //         }
        //     </div>
        // </div>
        <>
            <div className='container'>
                <div className='myBox'>
                    <div>
                        <h2 className="title">Sign Up</h2>
                    </div>
                    <form className="form-wrapper">
                        <div className="username">
                            <div className='row mb-3'>
                                <TextField type="text"
                                    error={usernameflag}
                                    className="form-control"
                                    label="Username"
                                    variant="outlined"
                                    name="username"
                                    value={username}
                                    onChange={handleChange}
                                />
                            </div>
                        </div>
                    </form>
                    <div className="webcam-container">
                        <div className="webcam-img">

                            {image === '' ? <Webcam
                                audio={false}
                                height={200}
                                ref={webcamRef}
                                screenshotFormat="image/jpeg"
                                width={220}
                                videoConstraints={videoConstraints}
                            /> : <img src={image} alt="" />}
                        </div>
                        <div>
                            {image !== '' ?
                                <button className="button-img" onClick={(e) => {
                                    e.preventDefault();
                                    setImage('')
                                }}
                                >Retake Image</button> :
                                <button className="button-img" onClick={(e) => {
                                    e.preventDefault();
                                    capture();
                                }}
                                >Capture</button>
                            }
                        </div>
                    </div>
                    <div>
                        <button className='button' onClick={handleFormSubmit}>Sign Up</button>
                    </div>
                </div>
            </div>
        </>
    );
}


