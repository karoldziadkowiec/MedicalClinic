import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { Link as RouterLink } from 'react-router-dom';

const AppNavbar = () => {
  return (
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand as={RouterLink} to="/">MedicalClinic</Navbar.Brand>
          <Navbar.Toggle aria-controls="responsive-navbar-nav" />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto blue-links">
              <Nav.Link as={RouterLink} to="/">Home</Nav.Link>
              <Nav.Link as={RouterLink} to="/patients">Patients</Nav.Link>
              <Nav.Link as={RouterLink} to="/new-patient">New Patient</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
  );
}

export default AppNavbar;