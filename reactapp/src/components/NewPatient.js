import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, Button, Alert, Row, Col, Modal } from 'react-bootstrap';
import Patient from '../models/Patient';
import PatientApi from '../services/api/PatientApi';
import '../App.css';
import '../styles/NewPatient.css';

const NewPatient = () => {
  const [patient, setPatient] = useState({ ...Patient });
  const [errorMessage, setErrorMessage] = useState('');
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();

  const moveToPatientsPage = () => {
    navigate('/patients');
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (name === 'city' || name === 'street' || name === 'zipCode') {
      setPatient(prevState => ({
        ...prevState,
        address: {
          ...prevState.address,
          [name]: value
        }
      }));
    } else {
      setPatient(prevState => ({
        ...prevState,
        [name]: value
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!patient.firstName || !patient.lastName || !patient.pesel || !patient.address.city || !patient.address.street || !patient.address.zipCode) {
      setErrorMessage('Please fill in all fields.');
      return;
    }

    if (!/^\d{11}$/.test(patient.pesel)) {
      setErrorMessage('Please enter a valid PESEL number (11 digits).');
      return;
    }

    try {
      await PatientApi.addNewPatient(patient);
      setShowModal(true);
    } catch (error) {
      setErrorMessage('Failed to add patient');
      console.error(error);
    }
  };

  return (
    <div className="NewPatient">
      <div className="forms-container">
        <h2>Add New Patient</h2>
        {errorMessage && <Alert variant="danger">{errorMessage}</Alert>}
        <Form onSubmit={handleSubmit}>
          <Row className="mb-3">
            <Col>
              <Form.Group>
                <Form.Label>First Name</Form.Label>
                <Form.Control type="text" name="firstName" value={patient.firstName} maxLength={20} placeholder="Enter first name" onChange={handleChange} required />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group>
                <Form.Label>Last Name</Form.Label>
                <Form.Control type="text" name="lastName" value={patient.lastName} maxLength={30} placeholder="Enter last name" onChange={handleChange} required />
              </Form.Group>
            </Col>
          </Row>
          <Form.Group className="mb-3">
            <Form.Label>PESEL</Form.Label>
            <Form.Control type="text" name="pesel" value={patient.pesel} maxLength={11} placeholder="Enter PESEL" onChange={handleChange} required />
          </Form.Group>
          <Row className="mb-3">
            <Col>
              <Form.Group>
                <Form.Label>City</Form.Label>
                <Form.Control type="text" name="city" value={patient.address.city} maxLength={20} placeholder="Enter city" onChange={(e) => handleChange({ target: { name: 'city', value: e.target.value } })} required />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group>
                <Form.Label>Street</Form.Label>
                <Form.Control type="text" name="street" value={patient.address.street} maxLength={30} placeholder="Enter street" onChange={(e) => handleChange({ target: { name: 'street', value: e.target.value } })} required />
              </Form.Group>
            </Col>
          </Row>
          <Form.Group className="mb-3">
            <Form.Label>Zip Code</Form.Label>
            <Form.Control type="text" name="zipCode" value={patient.address.zipCode} maxLength={6} placeholder="Enter zip code" onChange={(e) => handleChange({ target: { name: 'zipCode', value: e.target.value } })} required />
          </Form.Group>
          <Button type="submit" variant="success">Add Patient</Button>
        </Form>
        <Modal show={showModal} onHide={() => setShowModal(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Success</Modal.Title>
          </Modal.Header>
          <Modal.Body>Patient added successfully!</Modal.Body>
          <Modal.Footer>
            <Button variant="info" onClick={moveToPatientsPage}>Move to Patients</Button>
          </Modal.Footer>
        </Modal>
      </div>
    </div>
  );
};

export default NewPatient;