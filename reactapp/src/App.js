import React from 'react';
import './App.css';
import Navbar from './Navbar';
import Footer from './Footer';
import Home from './components/Home';
import Patients from './components/Patients';
import NewPatient from './components/NewPatient';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/patients" element={<Patients />} />
        <Route path="/new-patient" element={<NewPatient />} />
      </Routes>
      <Footer /> 
    </Router>
  );
}

export default App;