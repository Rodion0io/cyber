using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using hospital_api.Dates;
using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace hospital_api.Services;

public class DoctorServic : IDoctorServic
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountsContext _context;
    private readonly IJWTService _jwtService;
    
    public DoctorServic(IAccountRepository accountRepository, AccountsContext context, IJWTService jwtService)
    {
        _accountRepository = accountRepository;
        _context = context;
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

        if (checkAccaount != null)
        {
            throw new Exception($"с такой почтой пользователь существует!"); 
        }
        else
        {
            var checkSpeciality = _context.Specialities.FindAsync((newAccount.speciality).ToString()).Result;
            if (checkSpeciality is not null)
            {
                var hashingPassword = new PasswordHasher<Doctor>().HashPassword(newAccount, newAccount.password);
                newAccount.password = hashingPassword;
                await _accountRepository.Add(newAccount);
                Console.WriteLine($"password: {newAccount.password}, hashing password: {hashingPassword}");
            }
            else
            {
                Console.WriteLine("Такой специальности нет!");
                throw new Exception($"Speciality {newAccount.speciality} does not exist");
            }
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

    public async Task GetDataInClaim(string token)
    {
        var claimIdentifier = _jwtService.DecodeToken(token).Claims.ToArray()[2].Value;

        BlackListTokens model = new BlackListTokens(claimIdentifier, token);

        await _accountRepository.AddToBlackList(model);

    }

    public async Task<bool> InBlackList(string token)
    {
        return await _accountRepository.FindTokenInBlackList(token);
    }

    public DoctorModel GetDoctorInfa(string id)
    {
        
        var claimIdentifier = _jwtService.DecodeToken(id).Claims.ToArray()[2].Value;

        Doctor doctor = _accountRepository.FindDoctorById(claimIdentifier);
        
        DoctorModel result = new DoctorModel
        {
            id = (doctor.id).ToString(),
            createTime = (doctor.createTime).ToString(),
            name = doctor.name,
            birthday = (doctor.birthday).ToString(),
            gender = doctor.gender,
            email = doctor.email,
            phone = doctor.phone
        };

        return result;
    }
}