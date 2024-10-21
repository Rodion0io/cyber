using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task RegistrationPatient(PatientCreateModel model)
    {
        PatientModel patient = new()
        {
            name = model.name,
            birthday = model.birthday,
            gender = model.gender
        };

        await _patientRepository.AddPatient(patient);
    }
}