using System.Text;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace hospital_api;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(nameof(AuthSettings))
            .Get<AuthSettings>();
        
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
                };

                
                //Фрагмент выдал чат-гпт
                // Добавляем обработчик событий для проверки токенов в черном списке
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var doctorService = context.HttpContext.RequestServices.GetRequiredService<IDoctorServic>();

                        // Проверяем, есть ли токен в черном списке через сервис
                        if (await doctorService.InBlackList(token))
                        {
                            context.Fail("This token is blacklisted.");
                        }
                    }
                };
            });
        return serviceCollection;
    }
}