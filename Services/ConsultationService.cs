using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace hospital_api.Services;

public class ConsultationService : IConsultationService
{
    
    private readonly IConsultationRepository _consultationRepository;
    private readonly IInspectionRepository _inspectionRepository;

    public ConsultationService(IConsultationRepository consultationRepository, IInspectionRepository inspectionRepository)
    {
        _consultationRepository = consultationRepository;
        _inspectionRepository = inspectionRepository;
    }
    
    public async Task<ConsultationModel> GetConcreteConsultation(Guid id)
    {
        Consultation model = await _consultationRepository.GetConsultations(id);
        SpecialityModel speciality = await _consultationRepository.GetSpeciality(model.specialityId);
        List<Comment> comments = await _consultationRepository.GetComment(model.id);
        List<CommentModel> newCommentsList = new List<CommentModel>();

        foreach (var value in comments)
        {
            CommentModel commentModel = new CommentModel
            {
                id = value.id,
                createTime = value.createTime,
                modifiedDate = value.modifiedDate,
                content = value.content,
                authorId = value.authorId,
                author = value.author,
                parentId = value.parentId
            };
            newCommentsList.Add(commentModel);
        }

        ConsultationModel result = new ConsultationModel
        {
            id = model.id,
            createTime = model.createTime,
            inspectionId = model.inspectionId,
            speciality = speciality,
            comments = newCommentsList.ToArray()
        };

        return result;
    }
}