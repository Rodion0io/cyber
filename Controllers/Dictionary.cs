using System.ComponentModel.Design;
using hospital_api.Modules;
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
    
    [HttpGet("speciality")]
    public async Task<IActionResult> GetSpeciality([FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "pageNuber")] int pageNumber = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 1)
    {
        
        //вот так нельзя вызывать. движок метода GetFullSpeciaityTable
        var listSpeciality = await _dictionaryService.GetFullSpeciaityTable();

        // Это высчитывает значение count
        int totalPages = (int)Math.Ceiling(18 / (double)pageSize);
        
        
        //Почему не работает???
        // var items = listSpeciality.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        // Это уже сама пагинация
        var items = listSpeciality.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        PageInfoModel pagination = new PageInfoModel
        {
            size = pageSize,
            current = pageNumber,
            count = totalPages
        };

        SpecialtiesPagedListModel result = new SpecialtiesPagedListModel
        {
            specialties = items,
            pagintaion = pagination
        };

        return Ok(result);
    }

    [HttpGet("icd10")]
    public async Task<IActionResult> GetIcd10([FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "pageNuber")] int pageNumber = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 1)
    {
        var listIcd = await _dictionaryService.FullListIcd();
        int totalCount = await _dictionaryService.returnLenghtTable();
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var items = listIcd.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        PageInfoModel pagination = new PageInfoModel
        {
            size = pageSize,
            current = pageNumber,
            count = totalPages
        };

        Icd10SearchModel result = new Icd10SearchModel
        {
            records = items,
            pagintaion = pagination
        };
        
        return Ok(result);
    }

    [HttpGet("icd10/roots")]
    public async Task<IActionResult> GetIcd10Roots()
    {
        List<Icd10RecordModel> result = await _dictionaryService.FullListIcdRoots(); 
        
        return Ok(result);
    }
}