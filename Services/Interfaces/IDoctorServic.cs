using System.Security.Claims;
using hospital_api.Enums;
using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IDoctorServic
{
    public Task Registeration(DoctorRegisterModel model);
    public Task<TokenRespones> Login(string email, string password);
    public Task GetDataInClaim(string token);
    public Task<bool> InBlackList(string token);
    // Можно ли такого типа функцию создавать? или нужно через Task<>
    public Task<DoctorModel> GetDoctorInfa(string id);
    public Task ChangeDatas(DoctorEditModel model, string id);
}