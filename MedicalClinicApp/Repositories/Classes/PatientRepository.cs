using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories.Classes
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Patient>> GetAllPatients()
            => await Task.FromResult(_context.Patients.OrderBy(p => p.Id));

        public async Task<Patient> GetPatientById(int patientId)
            => await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId);


    }
}
