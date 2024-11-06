using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Services;

public class ConsultationService : IConsultationService
{
    
    private readonly IConsultationRepository _consultationRepository;
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly AccountsContext _context;

    public ConsultationService(IConsultationRepository consultationRepository, IInspectionRepository inspectionRepository, 
        IAccountRepository accountRepository, AccountsContext context)
    {
        _consultationRepository = consultationRepository;
        _inspectionRepository = inspectionRepository;
        _accountRepository = accountRepository;
        _context = context;
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

    public async Task<List<InspectionPreviewModel>> GetListInspectionForConsultation(Guid doctorId)
    {
        var doctor = await _accountRepository.FindDoctorById(doctorId.ToString());
        Guid doctorSpecialityId = doctor.speciality;
        List<InspectionPreviewModel> result = new List<InspectionPreviewModel>();
        List<Guid> listConsultations = await _context.Consultations.Where(i => i.specialityId == doctorSpecialityId)
            .Select(i => i.inspectionId).ToListAsync();

        result = await _context.Inspections.Where(i => listConsultations.Contains(i.id))
            .Select(i => new InspectionPreviewModel
            {
                id = i.id,
                createTime = i.createTime,
                previousId = i.previousInspectionId,
                date = i.date,
                conclusion = i.conclusion,
                doctorId = i.doctor,
                doctor = _context.Doctors.Where(x => x.id == i.doctor)
                    .Select(x => x.name).FirstOrDefault(),
                patientId = i.patient,
                patient = _context.Patients.Where(x => x.id == i.patient)
                    .Select(x => x.name).FirstOrDefault(),
                diagnosis = _context.Diagnosis.Where(x => x.inspectionId == i.id)
                    .Select(x => new DiagnosisModel
                    {
                        id = x.id,
                        createTime = x.createTime,
                        code = x.code,
                        name = x.name,
                        description = x.description,
                        type = x.type
                    }).FirstOrDefault(),
                
                hasChain = false,
                hasNested = false
                
            }).ToListAsync();

        return result;
    }
}