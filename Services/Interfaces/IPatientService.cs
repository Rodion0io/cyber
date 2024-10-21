using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IPatientService
{
    public Task RegistrationPatient(PatientCreateModel model);
}