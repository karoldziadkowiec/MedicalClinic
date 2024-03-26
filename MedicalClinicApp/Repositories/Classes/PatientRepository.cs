using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        public async Task<byte[]> GetPatientsCsvBytes()
        {
            var patients = await _context.Patients.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Patients");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "First name";
                worksheet.Cell(1, 3).Value = "Last name";
                worksheet.Cell(1, 4).Value = "PESEL";
                worksheet.Cell(1, 5).Value = "City";
                worksheet.Cell(1, 6).Value = "Street";
                worksheet.Cell(1, 7).Value = "Zip code";

                var row = 2;
                foreach (var patient in patients)
                {
                    worksheet.Cell(row, 1).Value = patient.Id;
                    worksheet.Cell(row, 2).Value = patient.FirstName;
                    worksheet.Cell(row, 3).Value = patient.LastName;
                    worksheet.Cell(row, 4).Value = patient.Pesel;

                    var existingAddress = await _context.Addresses.FindAsync(patient.AddressId);
                    if (existingAddress != null)
                    {
                        worksheet.Cell(row, 5).Value = patient.Address.City;
                        worksheet.Cell(row, 6).Value = patient.Address.Street;
                        worksheet.Cell(row, 7).Value = patient.Address.ZipCode;
                    }

                    row++;
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
