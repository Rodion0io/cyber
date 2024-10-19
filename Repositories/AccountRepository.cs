using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Repositories;

public class AccountRepository : IAccountRepository
{
    
    private readonly AccountsContext _context;

    public AccountRepository(AccountsContext context)
    {
        _context = context;
    }

    public DoctorRegisterModel[] ToDoctorRegisterModel(Doctor doctor)
    {
        return _context.Doctors.Select(x => new DoctorRegisterModel
        {
            name = x.name,
            password = x.password,
            email = x.email,
            birthday = x.birthday,
            gender = x.gender,
            phone = x.phone,
            speciality = x.speciality
        }).ToArray();
    } // Этот метод может здесь находиться???

    public DoctorModel[] ToDoctorModel(Doctor doctor)
    {
        return _context.Doctors.Select(x => new DoctorModel
        {
            id = x.id,
            createTime = x.createTime,
            name = x.name,
            birthday = x.birthday,
            gender = x.gender,
            email = x.email,
            phone = x.phone
        }).ToArray();
    }

    public async Task Add(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }

    public Doctor? FindDoctor(string email)
    {
        Doctor result = null;
        var db = _context.Doctors.ToList();
        foreach (var doct in db)
        {
            if (doct.email == email)
            {
                result =  doct;
                break;
            }
        }

        return result;
    }

    public async Task AddToBlackList(BlackListTokens tokens)
    {
        await _context.BlackListTokens.AddAsync(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> FindTokenInBlackList(string token)
    {
        bool value = await _context.BlackListTokens.AnyAsync(x => x.token == token);

        if (value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}