using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IPatientRepository
{
    public Task AddPatient(PatientModel model);
    public Task<PatientModel> FindPatient(string id);
    public Task<Inspection> FindInspection(Guid? id);
    public Task AddInspection(Inspection model);
    public Task AddComments(Comment model);
    public Task AddDiagnosis(Diagnosis model);
    public Task AddConsultation(Consultation model);
    public Task<Inspection[]> GetInspetionWithoutChild(Guid patientId);
    public Task<List<DiagnosisModel>> GetDiagnosisInspectionWithUotChild(Guid patientId, string? name);
    public Task<List<Guid>> GetPatientName(string partName);
}