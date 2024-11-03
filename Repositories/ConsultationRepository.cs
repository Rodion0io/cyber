using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Repositories;

public class ConsultationRepository : IConsultationRepository
{

    private readonly AccountsContext _context;

    public ConsultationRepository(AccountsContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetComment(Guid consultationId)
    {
        List<Comment> result = await _context.Comments.Where(i => i.consultationId == consultationId).ToListAsync();
        
        return result;
    }

    public async Task<SpecialityModel> GetSpeciality(Guid specialityId)
    {
        SpecialityModel result = await _context.Specialities.FirstOrDefaultAsync(i => i.id == specialityId);
        return result;
    }

    public async Task<Consultation> GetConsultations(Guid id)
    {
        Consultation consultation = await _context.Consultations.FirstOrDefaultAsync(i => i.id == id);

        return consultation;
    }
}