using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IDictionaryRepository
{
    public void Add();
    public Task<List<SpecialityModel>> getFullListSpeciality();
    public Task AddIcd10Models(List<Icd10Model> icd10Models);
    public Task<int> GetSizeTable();
    public Task<List<Icd10RecordModel>> getFullListIcd10();
    public Task<List<Icd10RecordModel>> getFullListIcd10Roots();
    public Task<string> getIcd10Name(Guid id);
    public Task<string> getIcd10Code(Guid id);
}