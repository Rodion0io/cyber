using System.ComponentModel.Design;
using System.Globalization;
using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hospital_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReportController : Controller
{
    
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpGet("icdrootsreport")]
    public async Task<ActionResult<IcdRootsReportModel>> GetIcdRootsReport([FromQuery(Name = "Start")] DateTime start,
        [FromQuery(Name = "End")] DateTime end, [FromQuery] List<Guid> icdRoots)
    {
        
        var res = await _reportService.GetReportModel(start, end, icdRoots);
        return Ok(res);
    }
}