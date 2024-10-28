using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

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
    
    public async Task<List<Icd10Model>> Icd10ModelsFromJson(string jsonFilePath)
    {
        string jsonString = await File.ReadAllTextAsync(jsonFilePath);
        
        var records = JsonConvert.DeserializeObject<List<Icd10Model>>(jsonString);
        
        return records;
    }
    
    public async Task AddIcd10(List<Icd10Model> records)
    {
        await _dictionaryRepository.AddIcd10Models(records);
    }
    
    public async Task<int> returnLenghtTable()
    {
        return await _dictionaryRepository.GetSizeTable();
    }
    
    //Так стоит делать?
    public async Task<List<Icd10RecordModel>> FullListIcd()
    {
        return await _dictionaryRepository.getFullListIcd10();
    }
    
    public async Task<List<Icd10RecordModel>> FullListIcdRoots()
    {
        return await _dictionaryRepository.getFullListIcd10Roots();
    }
}