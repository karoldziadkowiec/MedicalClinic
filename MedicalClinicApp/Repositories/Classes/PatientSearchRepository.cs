using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories.Classes
{
    public class PatientSearchRepository : IPatientSearchRepository
    {
        private readonly AppDbContext _context;

        public PatientSearchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string searchTerm)
            => await Task.FromResult(_context.Patients.Where(p => p.FirstName == searchTerm || p.LastName == searchTerm));

        public async Task<IEnumerable<Patient>> SearchPatientsPartial(string searchTerm)
        {
            return await _context.Patients
                .Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
