using DocumentFormat.OpenXml.Spreadsheet;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Route("api/patients/sort")]
    [ApiController]
    public class PatientSortController : ControllerBase
    {
        private readonly IPatientSortRepository _patientSortRepository;

        public PatientSortController(IPatientSortRepository patientSortRepository)
        {
            _patientSortRepository = patientSortRepository;
        }

        // GET: /api/patients/sort/last-name
        [HttpGet("last-name")]
        public async Task<ActionResult<IEnumerable<Patient>>> SortByLastName()
        {
            try
            {
                var patients = await _patientSortRepository.SortByLastName();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/patients/sort/pesel
        [HttpGet("pesel")]
        public async Task<ActionResult<IEnumerable<Patient>>> SortByPesel()
        {
            try
            {
                var patients = await _patientSortRepository.SortByPesel();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/patients/sort/city
        [HttpGet("city")]
        public async Task<ActionResult<IEnumerable<Patient>>> SortByCity()
        {
            try
            {
                var patients = await _patientSortRepository.SortByCity();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/patients/sort/zip-code
        [HttpGet("zip-code")]
        public async Task<ActionResult<IEnumerable<Patient>>> SortByZipCode()
        {
            try
            {
                var patients = await _patientSortRepository.SortByZipCode();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving patients: {ex.Message}");
            }
        }
    }
}
