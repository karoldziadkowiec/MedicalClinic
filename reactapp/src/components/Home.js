import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import '../App.css';
import '../styles/Home.css';

const Home = () => {
  const navigate = useNavigate();

  const moveToPatientsPage = () => {
    navigate('/patients');
  };

  const moveToNewPatientPage = () => {
    navigate('/new-patient');
  };

  return (
    <div className="Home">
      <h1><span className="medicalClinic">MedicalClinic</span></h1>
      <img src={require('../img/logo.png')} alt="logo" className="logo" />
      <h2>Welcome to MedicalClinic, your trusted destination for comprehensive patient data management.</h2>
      <h4>With MedicalClinic, you can securely store and access patient records.</h4>
      <h5>Our user-friendly interface and robust security measures ensure peace of mind while managing sensitive medical information.</h5>
      <p></p>
      <div className="button-container">
        <Button variant="info" onClick={moveToPatientsPage}>Patients</Button>
        <span className="sign"> | </span>
        <Button variant="success" onClick={moveToNewPatientPage}>New Patient</Button>
      </div>
    </div>
  );  
}

export default Home;