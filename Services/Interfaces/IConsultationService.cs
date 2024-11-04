using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IConsultationService
{
    public Task<ConsultationModel> GetConcreteConsultation(Guid id);
    public Task AddCommentConsultation(CommentCreateModel model, Guid consultationId,
        Guid docotrId, string doctorName);
}