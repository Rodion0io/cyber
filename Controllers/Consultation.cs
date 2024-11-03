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
    [HttpGet]
    public async Task<IActionResult> GetListConsultation()
    {
        return Ok();
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConsultation()
    {
        return Ok();
    }
    
}