using System.IdentityModel.Tokens.Jwt;
using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IJWTService
{
    public string GenerateToken(Doctor doctor);
    public JwtSecurityToken DecodeToken(string token);
}