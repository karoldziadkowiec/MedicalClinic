using MedicalClinicApp.Models;

namespace MedicalClinicApp.Services.Interfaces
{
    public interface IPatientExportService
    {
        Task<byte[]> GetPatientsCsvBytes();
    }
}