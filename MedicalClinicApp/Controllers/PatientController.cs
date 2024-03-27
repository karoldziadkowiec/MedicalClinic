using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET: /api/patients
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientRepository.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/patients/pagination
        [HttpGet("/pagination")]
        public async Task<ActionResult<List<Patient>>> GetPatients(int page = 1, int pageSize = 10)
        {
            try
            {
                var patients = await _patientRepository.GetPatientsByPagination(page, pageSize);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/patients/:id
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            try
            {
                var patient = await _patientRepository.GetPatientById(patientId);
                if (patient == null)
                {
                    return NotFound($"Patient with id {patientId} not found");
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patient: {ex.Message}");
            }
        }

        // POST: /api/patients
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
        if (patient == null)
        {
            return BadRequest();
        }

        try
        {
            await _patientRepository.AddPatient(patient);
            return Ok(patient);
        }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding patient: {ex.Message}");
            }
        }

        // PUT: /api/patients/:id
        [HttpPut("{patientId}")]
        public async Task<IActionResult> UpdatePatient(int patientId, Patient patient)
        {
            try
            {
                if (patientId != patient.Id)
                {
                    return BadRequest("Patient id mismatch");
                }

                var existingPatient = await _patientRepository.GetPatientById(patientId);
                if (existingPatient == null)
                {
                    return NotFound($"Patient with id {patientId} not found");
                }

                await _patientRepository.UpdatePatient(patient);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating patient: {ex.Message}");
            }
        }

        // DELETE: /api/patients/:id
        [HttpDelete("{patientId}")]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            try
            {
                var existingPatient = await _patientRepository.GetPatientById(patientId);
                if (existingPatient == null)
                {
                    return NotFound($"Patient with id {patientId} not found");
                }

                await _patientRepository.DeletePatient(patientId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error removing patient: {ex.Message}");
            }
        }
    }
}
