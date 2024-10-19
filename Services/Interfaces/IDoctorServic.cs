using System.Security.Claims;
using hospital_api.Enums;
using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IDoctorServic
{
    public Task Registeration(DoctorRegisterModel model);
    public TokenRespones Login(string email, string password);
    public Task GetDataInClaim(string token);
    public Task<bool> InBlackList(string token);

}