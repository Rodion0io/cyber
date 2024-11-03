using System.ComponentModel.Design;
using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hospital_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class Consultation : Controller
{
    
    private readonly IConsultationService _consultationService;

    public Consultation(IConsultationService consultationService)
    {
        _consultationService = consultationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetListConsultation([FromQuery] bool? grouped, [FromQuery] int? page,
        [FromQuery] int? pageNumber)
    {
        
        return Ok();
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConsultation([FromQuery] string id)
    {

        var result = await _consultationService.GetConcreteConsultation(Guid.Parse(id));
        return Ok(result);
    }
    
}