using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;

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
    
}