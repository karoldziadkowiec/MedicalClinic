using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Classes;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Route("api/patients/search/")]
    [ApiController]
    public class PatientSearchController : ControllerBase
    {
        private readonly IPatientSearchRepository _patientSearchRepository;

        public PatientSearchController(IPatientSearchRepository patientSearchRepository)
        {
            _patientSearchRepository = patientSearchRepository;
        }

        // GET: /api/patients/search/:searchTerm
        [HttpGet("{searchTerm}")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchPatients(string searchTerm)
        {
            try
            {
                var patients = await _patientSearchRepository.SearchPatients(searchTerm);
                if (patients == null)
                {
                    return NotFound();
                }

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/patients/partial/:searchTerm
        [HttpGet("partial/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchPatientsPartial(string searchTerm)
        {
            try
            {
                var patients = await _patientSearchRepository.SearchPatientsPartial(searchTerm);
                if (patients == null)
                {
                    return NotFound();
                }

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}