import React from 'react'

function Home() {
  return (
    <>
      <div className='container'>
        <div className="card text-center">
          <div className="card-header">
            Featured
          </div>
          <div className="card-body">
            <h5 className="card-title">Welcome to my page 😁</h5>
            <p className="card-text">This app is designed such that we can identify human faces with web camera which is known as detection. We can sign up user with their credentials and then detect human faces and store in database. Then after user sign up person can login to the page, without sign up user can't login. Wondering how does it work click on the button below.</p>
            <a href="/signup" className="btn btn-primary">Let's get started</a>
          </div>
          <div className="card-footer text-muted">
          </div>
        </div>
      </div>
    </>
  )
}

export default Home
