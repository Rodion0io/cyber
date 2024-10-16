using hospital_api.Enums;
using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IDoctorServic
{
    public Task Registeration(DoctorRegisterModel model);
    public TokenRespones Login(string email, string password);
    
}