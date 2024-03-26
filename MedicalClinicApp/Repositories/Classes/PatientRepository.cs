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
            => await Task.FromResult(_context.Patients.OrderBy(p => p.Pesel));

        public async Task<List<Patient>> GetPatientsByPagination(int page, int pageSize)
        {
            return await _context.Patients
                .OrderBy(p => p.Pesel)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientById(int patientId)
            => await _context.Patients.FindAsync(patientId);

        public async Task AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            var existingPatient = await _context.Patients.FindAsync(patient.Id);

            if (existingPatient != null)
            {
                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                existingPatient.Pesel = patient.Pesel;
                existingPatient.AddressId = patient.AddressId;
            }

            var existingAddress = await _context.Addresses.FindAsync(patient.AddressId);
            if (existingAddress != null)
            {
                existingAddress.City = patient.Address.City;
                existingAddress.Street = patient.Address.Street;
                existingAddress.ZipCode = patient.Address.ZipCode;
            }
            else
            {
                throw new ArgumentException("Address not found");
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
