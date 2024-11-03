using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using hospital_api.Enums;
using Microsoft.EntityFrameworkCore;


namespace hospital_api.Controllers
{
    [ApiController]
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPatientList([FromQuery] string? name, [FromQuery] List<Conclusion> conclusions,
            [FromQuery] SortPatient? sorting, [FromQuery] bool? scheduledVisits, [FromQuery] bool? onlyMine,
            [FromQuery, DefaultValue(1)] int page, [FromQuery, DefaultValue(5)] int size)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            Guid Id = Guid.Parse(_jwtService.DecodeToken((token).ToString()).Claims.ToArray()[2].Value);
            
            
            var result = await _patientService.GetFilteringPatient(name, conclusions, sorting, scheduledVisits, onlyMine, Id);
            return Ok(result);
        }
        
        [HttpPost("{id}/inspections")]
        [Authorize]
        public async Task<IActionResult> PostPatientInspection(Guid id, [FromBody] InspectionCreateModel model)
        {
            
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            Guid Id = Guid.Parse(_jwtService.DecodeToken((token).ToString()).Claims.ToArray()[2].Value);
            string name = _jwtService.DecodeToken((token).ToString()).Claims.ToArray()[0].Value;
            
            
            if (_patientService.checkPrevInspection(model) && !await _patientService.checkTimeNewInspection(model))
            {
                throw new ValidationException("Если диагноз 'выздоровление', дата следующего визита и дата смерти не могут быть установлены.");
            }
            else if (_patientService.CheckConclusion(model) == 1)
            {
                throw new ValidationException("Если диагноз 'выздоровление', дата следующего визита и дата смерти не могут быть установлены.");
            }
            else if (_patientService.CheckConclusion(model) == 2)
            {
                throw new ValidationException("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может");
            }
            else if (_patientService.CheckConclusion(model) == 3)
            {
                throw new ValidationException("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может"); 
            }
            else if (!_patientService.CheckTypeDiagnosis(model))
            {
                throw new ValidationException("Должен быть ровно один диагноз типа Main");
            }
            else if (!_patientService.CheckAllSpecialities(model))
            {
                throw new ValidationException("осмотр не может иметь несколько консультаций с одинаковой специальностью врача");
            }
            else if (await _patientService.CheckDethPatient(model))
            {
                throw new ValidationException("пациент уже умер");
            }
            else
            {
                await _patientService.AddInpection(model, id, Id, name);
            }
            return Ok();
        } 
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPatient(string id)
        {
            PatientModel result = await _patientService.GetPatient(id);
            return Ok(result);
        }

        [HttpGet("{id}/inspections/search")]
        [Authorize]
        public async Task<IActionResult> SearchPatientInspection(Guid id, [FromQuery] string? request)
        {
            
            InspectionShortModel[] inspections = await _patientService.GetInspectionWithoutChild(id, request);
            return Ok(inspections);
        }
    }
}