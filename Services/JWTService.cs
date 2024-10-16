using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hospital_api.Modules;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace hospital_api.Services;

public class JWTService(IOptions<AuthSettings> options)
{
    public string GenerateToken(Doctor doctor)
    {

        //Стоит ли в клэйме указать id?
        var claims = new List<Claim>
        {
            new Claim("name", doctor.name),
            new Claim("email", doctor.email),
            new Claim("id", (doctor.id).ToString())
        };
        
        // if (options == null || options.Value.SecretKey == null || options.Value.Expires == null)
        // {
        //     Console.WriteLine("Увы!");
        // }

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256
                )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}