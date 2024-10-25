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
builder.Services.AddScoped<DictionaryRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDictionaryRepository, DictionaryRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IDoctorServic, DoctorServic>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IDictionaryServic, DictionaryServic>();
builder.Services.AddScoped<IPatientService, PatientService>();

// Регистрация настроек
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

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
    var value = scope.ServiceProvider.GetRequiredService<DictionaryRepository>();
    value.Add();
}

// Инициализация данных из JSON-файла
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AccountsContext>();
//
//     var dictionaryServic = scope.ServiceProvider.GetRequiredService<IDictionaryServic>();
//     string jsonFilePath = "/Users/rodionrybko/RiderProjects/hospital_api/hospital_api/1.2.643.5.1.13.13.11.1005_2.27.json";
//     var list = await dictionaryServic.Icd10ModelsFromJson(jsonFilePath);
//     await dictionaryServic.AddIcd10(list);
// }

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();