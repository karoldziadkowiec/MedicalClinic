using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;

namespace MedicalClinicApp.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IQueryable<Patient>> GetAllPatients();
        Task<List<Patient>> GetPatientsByPagination(int page, int pageSize);
        Task<Patient> GetPatientById(int patientId);
        Task AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(int patientId);
    }
}