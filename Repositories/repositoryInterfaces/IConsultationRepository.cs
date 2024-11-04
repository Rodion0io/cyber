using hospital_api.Modules;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IConsultationRepository
{
    public Task<Consultation> GetConsultations(Guid id);
    public Task<SpecialityModel> GetSpeciality(Guid specialityId);
    public Task<List<Comment>> GetComment(Guid consultationId);
    public Task<bool> CheckConsultation(Guid consultationId);
    public Task<bool> CheckComment(Guid commentId);
    public Task AddNewComment(Comment newComment);
}