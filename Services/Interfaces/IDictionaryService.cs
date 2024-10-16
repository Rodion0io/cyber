using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IDictionaryServic
{
    public Task<List<SpecialityModel>> GetFullSpeciaityTable();
}