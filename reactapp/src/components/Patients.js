import React, { useState, useEffect } from 'react';
import { ListGroup, Button, Modal, Form, Dropdown, Row, Col } from 'react-bootstrap';
import PatientApi from '../services/api/PatientApi';
import '../App.css';
import '../styles/Patients.css';

const Patients = () => {
  const [patients, setPatients] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [showDetailsModal, setShowDetailsModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [selectedPatient, setSelectedPatient] = useState(null);
  const [editedPatient, setEditedPatient] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortType, setSortType] = useState('');

  useEffect(() => {
    const fetchPatients = async () => {
      try {
        const data = await PatientApi.getPatients({ currentPage, searchTerm, sortType });
        setPatients(data);
        setLoading(false);
      } catch (error) {
        console.error(error);
      }
    };

    fetchPatients();

    return () => {
      setPatients([]);
      setLoading(true);
    };
  }, [currentPage, searchTerm, sortType]);

  const nextPage = () => {
    setCurrentPage(prevPage => prevPage + 1);
  };

  const prevPage = () => {
    if (currentPage > 1) {
      setCurrentPage(prevPage => prevPage - 1);
    }
  };

  const handleDelete = async (id) => {
    try {
      await PatientApi.deletePatient(id);
      setPatients(prevPatients => prevPatients.filter(patient => patient.id !== id));
    } catch (error) {
      console.error(error);
    }
  };
 const handleDetails = (patient) => {
    setSelectedPatient(patient);
    setShowDetailsModal(true);
  };

  const handleCloseDetailsModal = () => {
    setShowDetailsModal(false);
    setSelectedPatient(null);
  };

  const handleEdit = (patient) => {
    setEditedPatient(patient);
    setShowEditModal(true);
  };

  const handleCloseEditModal = () => {
    setShowEditModal(false);
    setEditedPatient(null);
  };

  const handleSaveChanges = async () => {
    try {
      await PatientApi.updatePatient(editedPatient);
      setPatients(prevPatients =>
        prevPatients.map(p =>
          p.id === editedPatient.id ? editedPatient : p
        )
      );
      setShowEditModal(false);
    } catch (error) {
      console.error(error);
    }
  };

  const handleEditInputChange = (e) => {
    const { name, value } = e.target;
    setEditedPatient(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleInputChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const handleSearch = () => {
    setCurrentPage(1);
  };

  const handleSort = (sortType) => {
    setSortType(sortType);
    setCurrentPage(1);
  };

  const handleExportPatients = async () => {
    try {
      await PatientApi.exportPatients();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="Patients">
      <div className="list-container">
        <h2>Patients</h2>
        <div className="search-sort-container d-flex justify-content-between">
          <div className="search-container">
            <Form.Group controlId="formSearch">
              <div className="d-flex">
                <Form.Control type="text" placeholder="Search..." value={searchTerm} onChange={handleInputChange} />
                <Button variant="primary" onClick={handleSearch}>Search</Button>
              </div>
            </Form.Group>
          </div>
          <div className="export-sort-container d-flex">
            <div className="export-container mr-2">
              <Button variant="success" onClick={handleExportPatients}>Export Patients</Button>
            </div>
            <div className="sort-container">
              <Dropdown>
                <Dropdown.Toggle variant="dark" id="dropdown-basic">
                  Sort By
                </Dropdown.Toggle>

                <Dropdown.Menu>
                  <Dropdown.Item onClick={() => handleSort('last-name')}>Last Name</Dropdown.Item>
                  <Dropdown.Item onClick={() => handleSort('pesel')}>PESEL</Dropdown.Item>
                  <Dropdown.Item onClick={() => handleSort('city')}>City</Dropdown.Item>
                  <Dropdown.Item onClick={() => handleSort('zip-code')}>Zip Code</Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </div>
          </div>
        </div>
        {loading ? (
          <p>Loading...</p>
        ) : (
          <>
            <ListGroup>
              <ListGroup.Item variant="primary">
                <Row>
                  <Col>Name</Col>
                  <Col>PESEL</Col>
                  <Col></Col>
                </Row>
              </ListGroup.Item>
              {patients.map(patient => (
                <div key={patient.id} className="list-group-item">
                  <Row>
                    <Col>{patient.lastName} {patient.firstName}</Col>
                    <Col>{patient.pesel}</Col>
                    <Col>
                      <Button variant="info" onClick={() => handleDetails(patient)}>Details</Button>
                      <Button variant="dark" onClick={() => handleEdit(patient)}>Edit</Button>
                      <Button variant="danger" onClick={() => handleDelete(patient.id)}>Delete</Button>
                    </Col>
                  </Row>
                </div>
              ))}
            </ListGroup>
            <div className="pagination">
              <Button variant="info" onClick={prevPage} disabled={currentPage === 1}>Previous</Button>
              <Button variant="info" onClick={nextPage}>Next</Button>
            </div>
            <Modal show={showDetailsModal} onHide={handleCloseDetailsModal} centered>
              <Modal.Header closeButton>
                <Modal.Title>Patient Details: {selectedPatient?.firstName} {selectedPatient?.lastName}</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                {selectedPatient && (
                  <div>
                    <p><strong>First name:</strong> {selectedPatient.firstName}</p>
                    <p><strong>Last name:</strong> {selectedPatient.lastName}</p>
                    <p><strong>PESEL:</strong> {selectedPatient.pesel}</p>
                    <p><strong>City:</strong> {selectedPatient.address.city}</p>
                    <p><strong>Street:</strong> {selectedPatient.address.street}</p>
                    <p><strong>Zip code:</strong> {selectedPatient.address.zipCode}</p>
                  </div>
                )}
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={handleCloseDetailsModal}>Close</Button>
              </Modal.Footer>
            </Modal>

            <Modal show={showEditModal} onHide={handleCloseEditModal} centered>
              <Modal.Header closeButton>
                <Modal.Title>Edit Patient: {editedPatient?.firstName} {editedPatient?.lastName}</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                <Form>
                  <Form.Group controlId="formFirstName">
                    <Form.Label>First Name</Form.Label>
                    <Form.Control type="text" placeholder="Enter first name" name="firstName" maxLength={20} value={editedPatient?.firstName} onChange={handleEditInputChange} />
                  </Form.Group>
                  <Form.Group controlId="formLastName">
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control type="text" placeholder="Enter last name" name="lastName" maxLength={30} value={editedPatient?.lastName} onChange={handleEditInputChange} />
                  </Form.Group>
                  <Form.Group controlId="formCity">
                    <Form.Label>City</Form.Label>
                    <Form.Control type="text" placeholder="Enter city" name="address.city" maxLength={20} value={editedPatient?.address?.city} onChange={handleEditInputChange} />
                  </Form.Group>
                  <Form.Group controlId="formStreet">
                    <Form.Label>Street</Form.Label>
                    <Form.Control type="text" placeholder="Enter street" name="address.street" maxLength={30} value={editedPatient?.address?.street} onChange={handleEditInputChange} />
                  </Form.Group>
                  <Form.Group controlId="formZipCode">
                    <Form.Label>Zip Code</Form.Label>
                    <Form.Control type="text" placeholder="Enter zip code" name="address.zipCode" maxLength={6} value={editedPatient?.address?.zipCode} onChange={handleEditInputChange} />
                  </Form.Group>
                </Form>
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={handleCloseEditModal}>Close</Button>
                <Button variant="success" onClick={handleSaveChanges}>Save Changes</Button>
              </Modal.Footer>
            </Modal>
          </>
        )}
      </div>
    </div>
  );
};

export default Patients;