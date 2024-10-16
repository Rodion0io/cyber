using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace hospital_api.Services;

public class DoctorServic : IDoctorServic
{
    private readonly IAccountRepository _accountRepository;
    private readonly JWTService _jwtService;

    public DoctorServic(IAccountRepository accountRepository, JWTService jwtService)
    {
        _accountRepository = accountRepository;
        _jwtService = jwtService;
    }

    public async Task Registeration(DoctorRegisterModel model)
    {
        
        var newAccount = new Doctor
        {
            name = model.name,
            password = model.password,
            email = model.email,
            birthday = model.birthday,
            gender = model.gender,
            phone = model.phone,
            speciality = model.speciality
        };

        var checkAccaount = _accountRepository.FindDoctor(newAccount.email);

        if (checkAccaount is not null)
        {
            throw new Exception($"Doctor {newAccount.email} already exists");
        }
        else
        {
            var hashingPassword = new PasswordHasher<Doctor>().HashPassword(newAccount, newAccount.password);
            newAccount.password = hashingPassword;
            await _accountRepository.Add(newAccount);
            Console.WriteLine($"password: {newAccount.password}, hashing password: {hashingPassword}");
        }
    }

    public TokenRespones Login(string email, string password)
    {
        var account = _accountRepository.FindDoctor(email);

        if (account != null)
        {
            var result = new PasswordHasher<Doctor>().VerifyHashedPassword(account, account.password, password);
            if (result == PasswordVerificationResult.Success)
            {
                var toke = _jwtService.GenerateToken(account);
                return new TokenRespones{token = toke};
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid password");
            }
        }
        else
        {
            throw new UnauthorizedAccessException("Doctor not found");
        }
    }
}