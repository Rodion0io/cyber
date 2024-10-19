using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using hospital_api;
using hospital_api.Dates;
using hospital_api.Repositories;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Для сваггера
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    };
    c.AddSecurityRequirement(securityRequirement);
});

// БД
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AccountsContext>(options => options.UseNpgsql(connection));

// Регистрация репозиториев
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<SpecialityRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IDoctorServic, DoctorServic>();
builder.Services.AddScoped<IJWTService, JWTService>();

// Регистрация настроек
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddScoped<IDictionaryServic, DictionaryServic>();

// Настройка аутентификации
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

// Инициализация БД
// using var serviceScope = app.Services.CreateScope();
// var dbContext = serviceScope.ServiceProvider.GetService<AccountsContext>();
// dbContext?.Database.Migrate(); // Миграция

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Инициализация специальностей
using (var scope = app.Services.CreateScope())
{
    var value = scope.ServiceProvider.GetRequiredService<SpecialityRepository>();
    value.Add();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();