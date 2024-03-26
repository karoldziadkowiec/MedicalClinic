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

        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            try
            {
                await _patientRepository.AddPatient(patient);
                return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding patient: {ex.Message}");
            }
        }

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

        [HttpGet("csv")]
        public async Task<IActionResult> GetPatientsCsvFile()
        {
            try
            {
                var csvBytes = await _patientRepository.GetPatientsCsvBytes();
                return File(csvBytes, "application/octet-stream", "patients.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
