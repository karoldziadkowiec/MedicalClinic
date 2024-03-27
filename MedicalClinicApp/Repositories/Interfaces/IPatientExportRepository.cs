using MedicalClinicApp.Models;

namespace MedicalClinicApp.Repositories.Interfaces
{
    public interface IPatientExportRepository
    {
        Task<byte[]> GetPatientsCsvBytes();
    }
}