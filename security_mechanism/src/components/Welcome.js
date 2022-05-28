import React from 'react'
import { useLocation } from 'react-router-dom'

export default function Welcome() {
    const location = useLocation();
    return (
        <>
            <div className='container'><div className='welcome-text'>{location.state.name}</div></div>
        </>
    )
}
