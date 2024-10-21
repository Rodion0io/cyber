using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


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
                return BadRequest("error");
            }

            try
            {
                await _patientService.RegistrationPatient(model);
                return Ok("Patient was registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}