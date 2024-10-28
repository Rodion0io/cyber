using System.ComponentModel.DataAnnotations;
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
        private readonly IJWTService _jwtService;

        public PatientController(IPatientService patientService, IJWTService jwtService)
        {
            _patientService = patientService;
            _jwtService = jwtService;
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
        
        [HttpPost("{id}/inspections")]
        [Authorize]
        public async Task<IActionResult> PostPatientInspection(Guid id, [FromBody] InspectionCreateModel model)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            //Нужно поменять в модели доктора string на guid
            Guid Id = Guid.Parse(_jwtService.DecodeToken((token).ToString()).Claims.ToArray()[2].Value);
            string name = _jwtService.DecodeToken((token).ToString()).Claims.ToArray()[0].Value;
            await _patientService.AddInpection(model, Id, id, name);
            return Ok();
        } 
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPatient(string id)
        {
            PatientModel result = await _patientService.GetPatient(id);
            return Ok(result);
        }
    }
}