using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Repositories;


// Здесь можно реализовать создание всех специальностей или надо выносить куда-то?

public class DictionaryRepository : IDictionaryRepository
{
    private readonly AccountsContext _context;

    public DictionaryRepository(AccountsContext context)
    {
        _context = context;
    }

    // public String[] specialityArray =
    // {
    //     "Акушер-гинеколог", "Анестезиолог-реаниматолог", "Дерматовенеролог",
    //     "Инфекционист", "Кардиолог", "Невролог", "Онколог", "Отоларинголог", 
    //     "Офтальмолог", "Психиатр", "Психолог", "Рентгенолог", "Стоматолог",
    //     "Терапевт", "УЗИ-специалист", "Уролог", "Хирург", "Эндокринолог"
    // };
    public Dictionary<string, string> specialites = new Dictionary<string, string>
    {
            { "Акушер-гинеколог", "e8f93a49-b93f-47f0-a912-08dbffad6d0e" },
            { "Анестезиолог", "302d5c0c-5623-4810-a913-08dbffad6d0e" },
            { "Дерматовенеролог", "2c4b19f5-511d-4f27-a914-08dbffad6d0e" },
            { "Инфекционист", "4676b2f4-de54-4fce-a915-08dbffad6d0e" },
            { "Кардиолог", "b0f1d7c7-18e5-488b-a916-08dbffad6d0e" },
            { "Невролог", "6cb7fe40-bafe-49bc-a917-08dbffad6d0e" },
            { "Онколог", "75735935-74d3-4fa2-a918-08dbffad6d0e" },
            { "Отоларинголог", "ed1b936e-9c67-4da6-a919-08dbffad6d0e" },
            { "Офтальмолог", "87a9c38c-0d2d-4a52-a91a-08dbffad6d0e" },
            { "Психиатр", "5aa83ee6-9bb0-4afe-a91b-08dbffad6d0e" },
            { "Психолог", "6c20f45d-a7d1-4605-a91c-08dbffad6d0e" },
            { "Рентгенолог", "dfcc00ff-6595-41ad-a91d-08dbffad6d0e" },
            { "Стоматолог", "bf1f4b00-cf9c-48e4-a91e-08dbffad6d0e" },
            { "Терапевт", "9ea305d2-b1f8-405e-a91f-08dbffad6d0e" },
            { "УЗИ-специалист", "d82c6890-d26d-450b-a920-08dbffad6d0e" },
            { "Уролог", "2e73cece-5fda-4211-a921-08dbffad6d0e" },
            { "Хирург", "bec96e6f-8673-47c9-a922-08dbffad6d0e" },
            { "Эндокринолог", "15e97e43-315c-44b5-a923-08dbffad6d0e" }
    };

    
    // Тоже вопрос, метод должен быть как void???
    public void Add()
    {

        if (_context.Specialities.Count() != 0)
        {
            _context.Specialities.RemoveRange(_context.Specialities.ToList());
        }

        foreach (var speciality in specialites)
        {
            var newSpeciality = new SpecialityModel
            {
                name = speciality.Key,
                id = speciality.Value,
                createTime = DateTime.UtcNow
            };
            _context.Specialities.AddAsync(newSpeciality);
        }
        _context.SaveChanges();
    }

    public async Task<List<SpecialityModel>> getFullListSpeciality()
    {
        return await _context.Specialities.ToListAsync();
    }
}