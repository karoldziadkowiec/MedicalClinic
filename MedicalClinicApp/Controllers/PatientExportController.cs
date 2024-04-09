using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Classes;
using MedicalClinicApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientExportController : ControllerBase
    {
        private readonly IPatientExportService _patientExportService;

        public PatientExportController(IPatientExportService patientExportService)
        {
            _patientExportService = patientExportService;
        }

        // GET: /api/patients/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetPatientsCsvFile()
        {
            try
            {
                var csvBytes = await _patientExportService.GetPatientsCsvBytes();
                return File(csvBytes, "application/octet-stream", "patients.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}