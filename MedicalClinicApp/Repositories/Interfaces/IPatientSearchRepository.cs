using MedicalClinicApp.Models;

namespace MedicalClinicApp.Repositories.Interfaces
{
    public interface IPatientSearchRepository
    {
        Task<IEnumerable<Patient>> SearchPatients(string searchTerm);
        Task<IEnumerable<Patient>> SearchPatientsPartial(string searchTerm);
    }
}