import ApiURL from '../../constants/ApiConfig';

const PatientApi = {
  addNewPatient: async (patient) => {
    const response = await fetch(`${ApiURL}/patients`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(patient)
    });
  
    if (!response.ok) {
      throw new Error('Failed to add patient');
    }
  },
  
  getPatients: async ({ currentPage, searchTerm, sortType }) => {
    let url = `${ApiURL}/patients/pagination?page=${currentPage}&pageSize=7`;
    if (searchTerm) {
      url = `${ApiURL}/patients/search/partial/${searchTerm}`;
    } else if (sortType) {
      url = `${ApiURL}/patients/sort/${sortType}`;
    }
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error('Failed to fetch patients');
    }
    return response.json();
  },
  
  deletePatient: async (id) => {
    const response = await fetch(`${ApiURL}/patients/${id}`, {
      method: 'DELETE'
    });
    if (!response.ok) {
      throw new Error('Failed to delete patient');
    }
  },
  
  updatePatient: async (editedPatient) => {
    const response = await fetch(`${ApiURL}/patients/${editedPatient.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(editedPatient),
    });
    if (!response.ok) {
      throw new Error('Failed to update patient');
    }
    if (response.status === 204) {
      return;
    }
    return response.json();
  },
  
  exportPatients: async () => {
    const response = await fetch(`${ApiURL}/patients/csv`);
    if (!response.ok) {
      throw new Error('Failed to export patients');
    }
    const blob = await response.blob();
    const url = window.URL.createObjectURL(new Blob([blob]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'patients.csv');
    document.body.appendChild(link);
    link.click();
  }
};

export default PatientApi;