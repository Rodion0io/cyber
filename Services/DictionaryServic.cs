using hospital_api.Dates;
using Microsoft.EntityFrameworkCore;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class DictionaryServic : IDictionaryServic
{
    private readonly IDictionaryRepository _dictionaryRepository;

    public DictionaryServic(IDictionaryRepository dictionaryRepository)
    {
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task<List<SpecialityModel>> GetFullSpeciaityTable()
    {
        return await _dictionaryRepository.getFullListSpeciality();
    }
    
    
}