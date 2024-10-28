using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services.Interfaces;

public interface IPatientService
{
    public Task RegistrationPatient(PatientCreateModel model);
    public Task<PatientModel> GetPatient(string id);
    public bool checkPrevInspection(InspectionCreateModel model);
    public bool CheckConclusion(InspectionCreateModel model);
    public bool CheckTypeDiagnosis(InspectionCreateModel model);
    public Task<bool> CheckDethPatient(InspectionCreateModel model);
    public Task AddInpection(InspectionCreateModel model, Guid patintId, Guid doctorId, string doctorName);
}