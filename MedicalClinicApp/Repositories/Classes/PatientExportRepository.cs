using ClosedXML.Excel;
using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories.Classes
{
    public class PatientExportRepository : IPatientExportRepository
    {
        private readonly AppDbContext _context;

        public PatientExportRepository(AppDbContext context)
        {
            _context = context;
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
