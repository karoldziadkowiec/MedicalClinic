using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;

namespace MedicalClinicApp.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IQueryable<Patient>> GetAllPatients();
        Task<Patient> GetPatientById(int patientId);
    }
}
