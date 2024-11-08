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
        private readonly IInputOptions _inputOptions;

        public PatientController(IPatientService patientService, IJWTService jwtService,
            IInputOptions inputOptions)
        {
            _patientService = patientService;
            _jwtService = jwtService;
            _inputOptions = inputOptions;
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
        public async Task<ActionResult<PatientPagedListModel>> GetPatientList([FromQuery] string? name, [FromQuery] List<Conclusion> conclusions,
            [FromQuery] SortPatient? sorting, [FromQuery] bool? scheduledVisits, [FromQuery] bool? onlyMine,
            [FromQuery, DefaultValue(1)] int page, [FromQuery, DefaultValue(5)] int size)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            Guid Id = Guid.Parse(_jwtService.DecodeToken((token).ToString()).Claims.ToArray()[2].Value);
            
            var listSortedPatients = (await _patientService.GetFilteringPatient(name, conclusions, sorting, scheduledVisits, onlyMine, Id)).ToArray();
            
            int totalPages = (int)Math.Ceiling(listSortedPatients.Length / (double)size);
            var items = listSortedPatients.Skip((page - 1) * size).Take(size).ToList();

            PageInfoModel pagination = new PageInfoModel
            {
                size = page,
                current = size,
                count = totalPages
            };
            
            PatientPagedListModel result = new PatientPagedListModel
            {
                patients = items.ToArray(),
                pagination = pagination
            };
            
            
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
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "Если диагноз 'выздоровление', дата следующего визита и дата смерти не могут быть установлены."
            });
        }
        else if (_patientService.CheckConclusion(model) == 1)
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "Если диагноз 'болен', нужно назначить дату визита и дату смерти вводить не надо"
            });
        }
        else if (_patientService.CheckConclusion(model) == 2)
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "он мертв, ничего не назначай!"
            });
        }
        else if (_patientService.CheckConclusion(model) == 3)
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "здоров, ничего назначать не надо"
            });
        }
        else if (!_patientService.CheckTypeDiagnosis(model))
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "Должен быть ровно один диагноз типа Main"
            });
        }
        else if (!_patientService.CheckAllSpecialities(model))
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "осмотр не может иметь несколько консультаций с одинаковой специальностью врача"
            });
        }
        else if (await _patientService.CheckDethPatient(model))
        {
            return BadRequest(new ResponseModel
            {
                status = "error",
                message = "пациент уже умер"
            });
        }
        else
        {
            await _patientService.AddInpection(model, id, Id, name);
            return Ok();
        }
        
    }

        [HttpGet("{id}/inspections")]
        [Authorize]
        public async Task<ActionResult<InspectionPagedListModel>> GetListPatientInspections([FromQuery] Guid id, [FromQuery] bool grouped,
            [FromQuery] List<Guid> icdRoots, [FromQuery(Name = "page")] int pageNumber = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 5)
        {

            var listInspections = await _patientService.GetListPatientInspection(id);
            
            if (grouped != null)
            {
                listInspections = _inputOptions.GetFilteringGroupInspection(listInspections, grouped);
            }
            
            if (icdRoots.Count != 0)
            {
                listInspections = await _inputOptions.GetFilteringByParentCode(listInspections, icdRoots);
            }
            
            int totalPages = (int)Math.Ceiling(listInspections.Count / (double)pageSize);
            var items = listInspections.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            PageInfoModel pagination = new PageInfoModel
            {
                size = pageNumber,
                current = pageSize,
                count = totalPages
            };
            
            InspectionPagedListModel res = new InspectionPagedListModel
            {
                inspections = items.ToArray(),
                pagination = pagination
            };
            
            return Ok(res);
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PatientModel>> GetPatient(string id)
        {
            PatientModel result = await _patientService.GetPatient(id);
            return Ok(result);
        }

        [HttpGet("{id}/inspections/search")]
        [Authorize]
        public async Task<ActionResult<InspectionShortModel[]>> SearchPatientInspection(Guid id, [FromQuery] string? request)
        {
            
            InspectionShortModel[] inspections = await _patientService.GetInspectionWithoutChild(id, request);
            return Ok(inspections);
        }
    }
}