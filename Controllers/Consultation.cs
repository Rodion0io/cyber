using System.ComponentModel.Design;
using hospital_api.Modules;
using hospital_api.Services;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hospital_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class Consultation : Controller
{
    
    private readonly IConsultationService _consultationService;
    private readonly IJWTService _jwtService;

    public Consultation(IConsultationService consultationService, IJWTService jwtService)
    {
        _consultationService = consultationService;
        _jwtService = jwtService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetListConsultation([FromQuery] bool? grouped, [FromQuery] int? page,
        [FromQuery] int? pageNumber)
    {
        
        return Ok();
    }
    
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetConsultation([FromQuery] string id)
    {

        var result = await _consultationService.GetConcreteConsultation(Guid.Parse(id));
        return Ok(result);
    }

    [HttpPost("{id}/comment")]
    [Authorize]
    public async Task<IActionResult> AddCommentToConcreneteConsultation([FromQuery] Guid consultationId,
        [FromBody] CommentCreateModel model)
    {
        
        var authHeader = HttpContext.Request.Headers["Authorization"];
        // Console.WriteLine(authHeader);
        string token = authHeader.ToString().Split(" ")[1];
        // Console.WriteLine(token);
        Guid Id = Guid.Parse(_jwtService.DecodeToken(token).Claims.ToArray()[2].Value);
        string name = _jwtService.DecodeToken(token).Claims.ToArray()[0].Value;
        
        await _consultationService.AddCommentConsultation(model, consultationId, Id, name);
        return Ok();
    }
    
}