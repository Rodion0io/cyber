using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hospital_api.Controllers;


[Route("api/[controller]")]
public class Dictionary : Controller
{
    
    private readonly IDictionaryServic _dictionaryService;

    public Dictionary(IDictionaryServic dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }
    
    [Authorize]
    [HttpGet("speciality")]
    public async Task<IActionResult> Get()
    {
        var listSpeciality = await _dictionaryService.GetFullSpeciaityTable();
        return Ok(listSpeciality);
    }
    
}