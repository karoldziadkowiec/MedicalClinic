using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Classes;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientExportController : ControllerBase
    {
        private readonly IPatientExportRepository _patientExportRepository;

        public PatientExportController(IPatientExportRepository patientExportRepository)
        {
            _patientExportRepository = patientExportRepository;
        }

        // GET: /api/patients/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetPatientsCsvFile()
        {
            try
            {
                var csvBytes = await _patientExportRepository.GetPatientsCsvBytes();
                return File(csvBytes, "application/octet-stream", "patients.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}