using MedicalClinicApp.Models;

namespace MedicalClinicApp.Repositories.Interfaces
{
    public interface IPatientSortRepository
    {
        Task<IEnumerable<Patient>> SortByLastName();
        Task<IEnumerable<Patient>> SortByPesel();
        Task<IEnumerable<Patient>> SortByCity();
        Task<IEnumerable<Patient>> SortByZipCode();
    }
}