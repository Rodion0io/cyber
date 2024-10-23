using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services.Interfaces;

public interface IDictionaryServic
{
    public Task<List<SpecialityModel>> GetFullSpeciaityTable();
    public Task AddIcd10ModelsFromJson(string jsonFilePath);
    public Task<int> returnLenghtTable();
    public Task<List<Icd10RecordModel>> FullListIcd();
    public Task<List<Icd10RecordModel>> FullListIcdRoots();
}