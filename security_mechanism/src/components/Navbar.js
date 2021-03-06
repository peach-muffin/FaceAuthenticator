import React from 'react'
import { Link } from 'react-router-dom'

export default function Navbar() {
    return (
        <>
            <nav className="navbar navbar-expand-lg bg-dark">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/face-auth-front-end/">Face Authenticator</a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            <Link className="nav-link active" aria-current="page" to="/face-auth-front-end/signup">Sign Up</Link>
                            <Link className="nav-link" to="/face-auth-front-end/verify-using-face">Verification</Link>
                            <Link className="nav-link" to="/face-auth-front-end/login">Sign In</Link>
                        </div>
                    </div>
                </div>
            </nav>
        </>
    )
}
