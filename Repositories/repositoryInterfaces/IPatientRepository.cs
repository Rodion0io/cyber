using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IPatientRepository
{
    public Task AddPatient(PatientModel model);
    public Task<PatientModel> FindPatient(string id);
}