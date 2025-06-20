using System.Globalization;
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
using DotNetEnv;  // Добавьте using для DotNetEnv

var builder = WebApplication.CreateBuilder(args);

// Загрузка переменных из values.env
Env.Load("values.env");  // <-- вызов тут, сразу после создания builder

// Теперь получаем строку подключения из переменной окружения
var connection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// Если хотите — для отладки можно вывести в лог или консоль

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



//Поставил формат времени
var cultureInfo = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Регистрация DbContext с использованием строки из env
builder.Services.AddDbContext<AccountsContext>(options => options.UseNpgsql(connection));

// Регистрация репозиториев
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<DictionaryRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDictionaryRepository, DictionaryRepository>();
builder.Services.AddScoped<IInspectionRepository, InspectionRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IDoctorServic, DoctorServic>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IDictionaryServic, DictionaryServic>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IInputOptions, InputOptions>();
builder.Services.AddScoped<IReportService, ReportService>();

// Регистрация настроек
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

// Настройка аутентификации
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

// остальной код без изменений
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var value = scope.ServiceProvider.GetRequiredService<DictionaryRepository>();
    value.Add();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
