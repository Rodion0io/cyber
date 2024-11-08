using hospital_api.Dates;
using hospital_api.Enums;
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
        var result = await _context.Patients.FindAsync(Guid.Parse(id));

        if (result == null)
        {
            throw new Exception("Patient not found");
        }
        else
        {
            return result;
        }
        
    }

    public async Task<PatientModel> FindPatientByInspection(Guid id)
    {
        PatientModel result = await _context.Patients.FindAsync(id);
        return result;
    }

    public async Task<Inspection> FindInspection(Guid? id)
    {
        var result = await _context.Inspections.FirstOrDefaultAsync(i => i.id == id);
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

    public async Task<Inspection[]> GetInspetionWithoutChild(Guid patientId)
    {
        Inspection[] result = await _context.Inspections.Where(i => i.patient == patientId).ToArrayAsync();

        return result;
    }

    public async Task<List<DiagnosisModel>> GetDiagnosisInspectionWithUotChild(Guid inspectionId, string? name)
    {
        
        List<DiagnosisModel> listDiagnosis = await _context.Diagnosis
            .Where(i => 
                i.inspectionId == inspectionId && 
                i.type == DiagnosisType.Main &&
                (name == null || i.name.Contains(name) || i.code.Contains(name)))
            .Select(i => new DiagnosisModel
            {
                id = i.id,
                createTime = i.createTime,
                code = i.code,
                name = i.name,
                description = i.description,
                type = i.type
            })
            .ToListAsync();

        
        
        // Diagnosis result = _context.Diagnosis.Where(i =>
        //     i.inspectionId == inspectionId && i.type == DiagnosisType.Main &&
        //     (i.name.Contains(name) || i.code.Contains(name)));

        return listDiagnosis;
    }

    public async Task<List<Guid>> GetPatientName(string partName)
    {
        List<Guid> patients = new List<Guid>();

        patients = await _context.Patients.Where(i => i.name.Contains(partName))
            .Select(i => i.id).ToListAsync();

        return patients;
    }
}