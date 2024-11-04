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

    public async Task<bool> CheckConsultation(Guid consultationId)
    {
        bool result = await _context.Consultations.AnyAsync(i => i.id == consultationId);
        
        return result;
    }

    public async Task<bool> CheckComment(Guid commentId)
    {
        bool result = await _context.Comments.AnyAsync(i => i.id == commentId);

        return result;
    }

    public async Task AddNewComment(Comment newComment)
    {
        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> GetCommentModel(Guid commentId)
    {
        var comm = await _context.Comments.FirstOrDefaultAsync(i => i.id == commentId);

        if (comm != null)
        {
            return comm;
        }
        else
        {
            throw new Exception("Такого комментария нет!");
        }
    }

    public async Task UpdateContent(string newText, Comment model)
    {
        model.content = newText;
        model.modifiedDate = DateTime.Now.ToUniversalTime();
        await _context.SaveChangesAsync();
    }
}