using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using hospital_api.Enums;


namespace hospital_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionController : Controller
    {

        private readonly IInspectionService _inspectionService;
        private readonly IJWTService _jwtService;

        public InspectionController(IInspectionService inspectionService, IJWTService jwtService)
        {
            _inspectionService = inspectionService;
            _jwtService = jwtService;
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInspection(Guid id)
        {
            
            var authHeader = HttpContext.Request.Headers["Authorization"];
            // Console.WriteLine(authHeader);
            string token = authHeader.ToString().Split(" ")[1];
            // Console.WriteLine(token);
            string Id = _jwtService.DecodeToken(token).Claims.ToArray()[2].Value;
            // Console.WriteLine(Id);

            var result = await _inspectionService.GetInspection(id, Guid.Parse(Id));

            if (result == null)
            {
                return BadRequest("Такого осмотра нет!");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditInspection([FromBody] InspectionEditModel model, string id)
        {
            
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            string Id = _jwtService.DecodeToken(token).Claims.ToArray()[2].Value;
            
            await _inspectionService.EditInspection(Guid.Parse(id), Guid.Parse(Id), model);
            
            return Ok();
        }

        [HttpGet("{id}/chain")]
        [Authorize]
        public async Task<IActionResult> GetInspectionChain(Guid id)
        {

            if (await _inspectionService.CheckValidInspection(id))
            {
                var result = await _inspectionService.GetInspectionChain(id);
                return Ok(result);
            }
            
            return Ok();
        }
    }
}