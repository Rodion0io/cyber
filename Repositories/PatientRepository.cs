using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AccountsContext _context;

    public PatientRepository(AccountsContext context)
    {
        _context = context;
    }

    public async Task AddPatient(PatientModel model)
    {
        await _context.Patients.AddAsync(model);
        await _context.SaveChangesAsync();
    }
}