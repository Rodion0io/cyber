using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<PatientModel> FindPatient(string id)
    {
        PatientModel result = await _context.Patients.FindAsync(Guid.Parse(id));
    
        return result;
    }

    public async Task<PatientModel> FindPatientByInspection(Guid id)
    {
        PatientModel result = await _context.Patients.FindAsync(id);
        return result;
    }

    public async Task<Inspection> FindInspection(Guid? id)
    {
        var result = await _context.Inspections.FindAsync(id);
        if (result != null)
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task AddInspection(Inspection model)
    {
        await _context.Inspections.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task AddComments(Comment model)
    {
        await _context.Comments.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task AddDiagnosis(Diagnosis model)
    {
        await _context.Diagnosis.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task AddConsultation(Consultation model)
    {
        await _context.Consultations.AddAsync(model);
        await _context.SaveChangesAsync();
    }
}