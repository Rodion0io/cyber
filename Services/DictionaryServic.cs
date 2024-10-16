using hospital_api.Dates;
using Microsoft.EntityFrameworkCore;
using hospital_api.Modules;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class DictionaryServic : IDictionaryServic
{
    private readonly AccountsContext _context;

    public DictionaryServic(AccountsContext context)
    {
        _context = context;
    }

    public async Task<List<SpecialityModel>> GetFullSpeciaityTable()
    {
        return await _context.Specialities.ToListAsync();
    }
}