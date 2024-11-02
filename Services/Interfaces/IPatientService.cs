using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services.Interfaces;

public interface IPatientService
{
    public Task RegistrationPatient(PatientCreateModel model);
    public Task<PatientModel> GetPatient(string id);
    public bool checkPrevInspection(InspectionCreateModel model);
    public Task<bool> checkTimeNewInspection(InspectionCreateModel model);
    public int CheckConclusion(InspectionCreateModel model);
    public bool CheckTypeDiagnosis(InspectionCreateModel model);
    public bool CheckAllSpecialities(InspectionCreateModel model);
    public Task<bool> CheckDethPatient(InspectionCreateModel model);
    public Task<string> AddInpection(InspectionCreateModel model, Guid patintId, Guid doctorId, string doctorName);
    public Task<InspectionShortModel[]> GetInspectionWithoutChild(Guid patientId, string? partName);
}