using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services.Interfaces;

public interface IDictionaryServic
{
    public Task<List<SpecialityModel>> GetFullSpeciaityTable();
    public Task<List<Icd10Model>> Icd10ModelsFromJson(string jsonFilePath);
    public Task AddIcd10(List<Icd10Model> records);
    public Task<int> returnLenghtTable();
    public Task<List<Icd10RecordModel>> FullListIcd();
    public Task<List<Icd10RecordModel>> FullListIcdRoots();
}