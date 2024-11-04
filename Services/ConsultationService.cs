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
        var model = await _consultationRepository.GetConsultations(id);

        if (model == null)
        {
            throw new Exception("Такой консультации нет!");
        }
        else
        {
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

    public async Task AddCommentConsultation(CommentCreateModel model, Guid consultationId,
        Guid docotrId, string doctorName)
    {
        if (await _consultationRepository.CheckConsultation(consultationId))
        {
            Comment newComment = new Comment
            {
                content = model.content,
                authorId = docotrId,
                author = doctorName,
                parentId = (await _consultationRepository.CheckComment(model.parentId) != false ? model.parentId : null),
                consultationId = consultationId
            };
            await _consultationRepository.AddNewComment(newComment);
        }
        else
        {
            throw new Exception("Консультации такой нет!");
        }
    }

    public async Task UpdateComment(InspectionCommentCreateModel model, Guid commentId, Guid doctorId)
    {
        if (await _consultationRepository.CheckComment(commentId) != false)
        {
            var x = await _consultationRepository.GetCommentModel(commentId);

            if (x != null)
            {
                if (x.authorId == doctorId)
                {
                    await _consultationRepository.UpdateContent(model.content, x);
                }
                else
                {
                    throw new Exception("Вы не можете изменить данный комментарий!");
                }
            }
            else
            {
                throw new Exception("Ошибка");
            }
        }
        else
        {
            throw new Exception("Комментария с таким Id нет!");
        }
    }
}