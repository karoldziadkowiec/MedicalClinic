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
        {
            return await Task.FromResult(_context.Patients
                .Include(p => p.Address)
                .OrderBy(p => p.Pesel));
        }

        public async Task<List<Patient>> GetPatientsByPagination(int page, int pageSize)
        {
            return await _context.Patients
                .Include(p => p.Address)
                .OrderBy(p => p.Pesel)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientById(int patientId)
        {
            return await _context.Patients
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }

        public async Task AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            var existingPatient = await _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == patient.Id);

            if (existingPatient != null)
            {
                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                existingPatient.Pesel = patient.Pesel;
                existingPatient.Address.City = patient.Address.City;
                existingPatient.Address.Street = patient.Address.Street;
                existingPatient.Address.ZipCode = patient.Address.ZipCode;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeletePatient(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}