using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories.Classes
{
    public class PatientSortRepository : IPatientSortRepository
    {
        private readonly AppDbContext _context;

        public PatientSortRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> SortByLastName()
            => await _context.Patients.OrderBy(p => p.LastName).ToListAsync();

        public async Task<IEnumerable<Patient>> SortByPesel()
            => await _context.Patients.OrderBy(p => p.Pesel).ToListAsync();

        public async Task<IEnumerable<Patient>> SortByCity()
            => await _context.Patients.OrderBy(p => p.Address.City).ToListAsync();

        public async Task<IEnumerable<Patient>> SortByZipCode()
            => await _context.Patients.OrderBy(p => p.Address.ZipCode).ToListAsync();
    }
}
