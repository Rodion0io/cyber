using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IDictionaryRepository
{
    public void Add();
    public Task<List<SpecialityModel>> getFullListSpeciality();
    public Task<IActionResult> SendIcdInTable(List<>);
}