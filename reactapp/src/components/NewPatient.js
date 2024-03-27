import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, Button, Alert, Row, Col, Modal } from 'react-bootstrap';
import '../App.css';
import '../styles/NewPatient.css';

const AddPatient = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [pesel, setPESEL] = useState('');
  const [street, setStreet] = useState('');
  const [city, setCity] = useState('');
  const [zipCode, setZipCode] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();

  const moveToPatientsPage = () => {
    navigate('/patients');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!firstName || !lastName || !pesel || !street || !city || !zipCode) {
      setErrorMessage('Please fill in all fields.');
      return;
    }

    if (!/^\d{11}$/.test(pesel)) {
      setErrorMessage('Please enter a valid PESEL number (11 digits).');
      return;
    }

    const data = {
      firstName: firstName,
      lastName: lastName,
      pesel: pesel,
      address: {
        city: city,
        street: street,
        zipCode: zipCode
      }
    };

    try {
      const response = await fetch('https://localhost:7241/api/patients', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });

      if (!response.ok) {
        throw new Error('Failed to add patient');
      }

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
                <Form.Control type="text" value={firstName} maxLength={20} placeholder="Enter first name" onChange={(e) => setFirstName(e.target.value)} required />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group>
                <Form.Label>Last Name</Form.Label>
                <Form.Control type="text" value={lastName} maxLength={30} placeholder="Enter last name" onChange={(e) => setLastName(e.target.value)} required />
              </Form.Group>
            </Col>
          </Row>
          <Form.Group className="mb-3">
            <Form.Label>PESEL</Form.Label>
            <Form.Control type="text" value={pesel} maxLength={11} placeholder="Enter PESEL" onChange={(e) => setPESEL(e.target.value)} required />
          </Form.Group>
          <Row className="mb-3">
            <Col>
              <Form.Group>
                <Form.Label>City</Form.Label>
                <Form.Control type="text" value={city} maxLength={20} placeholder="Enter city" onChange={(e) => setCity(e.target.value)} required />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group>
                <Form.Label>Street</Form.Label>
                <Form.Control type="text" value={street} maxLength={30} placeholder="Enter street" onChange={(e) => setStreet(e.target.value)} required />
              </Form.Group>
            </Col>
          </Row>
          <Form.Group className="mb-3">
            <Form.Label>Zip Code</Form.Label>
            <Form.Control type="text" value={zipCode} maxLength={6} placeholder="Enter zip code" onChange={(e) => setZipCode(e.target.value)} required />
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

export default AddPatient;