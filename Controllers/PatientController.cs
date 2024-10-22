using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace hospital_api.Controllers
{
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] PatientCreateModel model)
        {
            if (model == null)
            {
                return BadRequest("Model is null");
            }

            try
            {
                await _patientService.RegistrationPatient(model);
                return Ok("Patient was registered");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred: " + ex.Message);
            }
        }

        
    }
}