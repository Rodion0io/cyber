using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


namespace hospital_api.Controllers
{
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
            Console.WriteLine(authHeader);
            string token = authHeader.ToString().Split(" ")[1];
            Console.WriteLine(token);
            string Id = _jwtService.DecodeToken(token).Claims.ToArray()[2].Value;
            Console.WriteLine(Id);

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
    }
}