using hospital_api.Modules;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IAccountRepository
{
    public Task Add(Doctor doctor);

    public Doctor? FindDoctor(string email);
    
    public DoctorRegisterModel[] ToDoctorRegisterModel(Doctor doctor); // Вопрос зедсь это можно реализовывать????

    public Task AddToBlackList(BlackListTokens tokens);

    public Task<bool> FindTokenInBlackList(string token);
}