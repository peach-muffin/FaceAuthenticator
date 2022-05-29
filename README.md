This project is Face Authenticator which demonstrates the following

 1. Created backend with ASP .NET Core Web API
 2. Created frontend with React JS
 3. Used MySQL for database
 4. Azure blob for storing images
 5. Azure Face API for detecting and training the images captured
 6. Making HTTP calls
 7. Communicating between backend and frontend
 8. Using basic routing in react
 9. Sign up and sign in mechanism with face authentication

# FaceAuthenticator
Backend code which is built in **ASP .NET Core Web API**

This repository contains a controller "AuthController" for user where we can post API of sign up, sign in, and registering user with face images.

In  [FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator)/[FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator/tree/master/FaceAuthenticator)/[Controllers](https://github.com/peach-muffin/FaceAuthenticator/tree/master/FaceAuthenticator/Controllers)/**AuthController.cs** write Azure Face API subscription key and end point in place of **"enter subscription key"** and **"endpoint to be entered here"** respectively.

It has data access layer "AuthDL" where it is storing data in MySQL database and fetching data while sign in.

It has  [FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator)/[FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator/tree/master/FaceAuthenticator)/[Services](https://github.com/peach-muffin/FaceAuthenticator/tree/master/FaceAuthenticator/Services)/**BlobService.cs** for uploading images to blob storage.

In [FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator)/[FaceAuthenticator](https://github.com/peach-muffin/FaceAuthenticator/tree/master/FaceAuthenticator)/**appsettings.json** write the connection string of MySQL in **"MySqlDBConnection"**. And write connection string of Azure blob storage in **"BlobConnectionString"** and container name of the blob in **"ContainerName"**.

### Application URL
I have deployed this on Azure and here is the link of this
[kakulengagefaceauthenticatorv1.azurewebsites.net](https://kakulengagefaceauthenticatorv1.azurewebsites.net/)

# security_mechanism
Front end code which is built in **React JS**
## Prerequisities
Install Node JS
Refer to  [https://nodejs.org/en/](https://nodejs.org/en/) to install Node JS

**Install create-react-app**
Install create-react-app npm package globally. This will help to easily run the project and build the source files easily. Use the following command to install create-react-app

    npm install -g create-react-app

To install all the npm packages go into the project folder and type the following command to install npm packages

    npm install
   In order to run the application type the following command
   

    npm start
   The application runs on [localhost](http://localhost:3000/)
   
**Application design**

 - HTTP client 
 - axios client is used to make HTTP calls

### Application URL
I have deployed this on Github and here is the link of this
[Face Authenticator (peach-muffin.github.io)](https://peach-muffin.github.io/face-auth-front-end/)

## Working of Application
![image](https://user-images.githubusercontent.com/73070520/170882424-07f0ae00-957a-4ff4-bb99-e98e8c06f661.png)


## Architecture
 - User interact with interface built with the help of React JS 
 - Fill the credentials at sign up it will get stored in database
 - User provides its image and it gets stored in Azure blob storage
 - URL from blob storage will be required to train face with Azure face api
 - During sign in, user provides its image and it communicate with the backend to verify the person
 - Which takes one or more face IDs from persisted face and it gives a list of person objects that each face might belong to.
![image](https://user-images.githubusercontent.com/73070520/170882331-80ea2e43-b318-421b-8571-9b15e4a06336.png)
