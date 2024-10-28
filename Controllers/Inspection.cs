using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


namespace hospital_api.Controllers
{
    [Route("api/[controller]")]
    public class Inspection : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspection(Guid id)
        {
            return Ok();
        }
    }
}