using System.ComponentModel.Design;
using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hospital_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReportController : Controller
{
    [HttpGet("icdrootsreport")]
    public async Task<ActionResult<IcdRootsReportModel>> GetIcdRootsReport([FromQuery(Name = "Start")] DateTime start,
        [FromQuery(Name = "End")] DateTime end, [FromQuery] List<Guid> icdRoots)
    {
        return Ok();
    }
}