import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from '../components/Home';
import Patients from '../components/Patients';
import NewPatient from '../components/NewPatient';

const Routing = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/patients" element={<Patients />} />
      <Route path="/new-patient" element={<NewPatient />} />
    </Routes>
  );
}

export default Routing;