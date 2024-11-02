using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDictionaryRepository _dictionaryRepository;

    public PatientService(IPatientRepository patientRepository,
        IDictionaryRepository dictionaryRepository)
    {
        _patientRepository = patientRepository;
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task RegistrationPatient(PatientCreateModel model)
    {
        PatientModel patient = new()
        {
            name = model.name,
            birthday = model.birthday,
            gender = model.gender
        };
    
        await _patientRepository.AddPatient(patient);
    }
    
    public async Task<PatientModel> GetPatient(string id)
    {
        return await _patientRepository.FindPatient(id);
    }

    public async Task<string> AddInpection(InspectionCreateModel model, Guid patintId, Guid doctorId, string doctorName)
    {
        
        Inspection newInspection = new Inspection
        {
            date = model.date,
            anamnesis = model.anamnesis,
            complaints = model.complaints,
            treatment = model.treatment,
            conclusion = model.conclusion,
            nextVisitDate = model.nextVisitDate,
            deathDate = model.deathDate,
            previousInspectionId = model.previousInspectionId,
            patient = patintId,
            doctor = doctorId
        };

        await _patientRepository.AddInspection(newInspection);

        newInspection.baseInspectionId = (newInspection.previousInspectionId == null ? newInspection.id : 
            (await _patientRepository.FindInspection(newInspection.previousInspectionId)).baseInspectionId);
        
        foreach (var diagnosis in model.diagnosis)
        {
            Diagnosis newDiagnosis = new Diagnosis
            {
                code = await _dictionaryRepository.getIcd10Code((diagnosis.icdDiagnosisId)),
                name = await _dictionaryRepository.getIcd10Name((diagnosis.icdDiagnosisId)),
                description = diagnosis.description,
                type = diagnosis.type,
                icdDiagnosisId = diagnosis.icdDiagnosisId,
                inspectionId = newInspection.id
            };

            await _patientRepository.AddDiagnosis(newDiagnosis);
        }
        
        foreach (var comment in model.consultations)
        {
            Consultation newConsultation = new Consultation
            {
                inspectionId = newInspection.id,
                specialityId = comment.specialityId
            };
            
            await _patientRepository.AddConsultation(newConsultation);
        
            Comment newComment = new Comment
            {
                content = comment.comment.content,
                authorId = doctorId,
                author = doctorName,
                consultationId = newConsultation.id
            };
            await _patientRepository.AddComments(newComment);
        }

        return (newInspection.id).ToString();
    }

    // //Так можно задвать тип функции? Или нужно использовать Task?
    public bool checkPrevInspection(InspectionCreateModel model)
    {
        if (model.previousInspectionId != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> checkTimeNewInspection(InspectionCreateModel model)
    {

        if (model.previousInspectionId != null)
        {
            var prevTime = await _patientRepository.FindInspection(model.previousInspectionId);
    
            if (prevTime != null)
            {
                int compareTime = DateTime.Compare(model.date, prevTime.date);
                if (compareTime > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    
    public int CheckConclusion(InspectionCreateModel model)
    {
        if ((model.conclusion == Conclusion.Disease && model.nextVisitDate == null && model.deathDate == null) ||
            (model.conclusion == Conclusion.Disease && model.nextVisitDate == null && model.deathDate  != null))
        {
            // return BadRequest("Если диагноз 'выздоровление', дата следующего визита и дата смерти не могут быть установлены.")
            return 1;
        }
        else if ((model.conclusion == Conclusion.Death && model.nextVisitDate != null && model.deathDate  == null) ||
                 (model.conclusion == Conclusion.Death && model.nextVisitDate != null && model.deathDate  != null))
        {
            // return BadRequest("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может");
            return 2;
        }
        else if (model.conclusion == Conclusion.Recovery && (model.nextVisitDate != null || model.deathDate != null))
        {
            // return BadRequest("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может");
            return 3;
        }
        else
        {
            return 4;
        }
    }
    
    public bool CheckTypeDiagnosis(InspectionCreateModel model)
    {
        int count = 0;
        foreach (var diagnos in model.diagnosis)
        {
            if (diagnos.type == DiagnosisType.Main)
            {
                count++;
            }
    
            if (count > 1)
            {
                break;
            }
        }
    
        return count == 1;
    }
    
    public bool CheckAllSpecialities(InspectionCreateModel model)
    {
        int flag = 1;
        string prevSpecialityId = null;
        foreach (var value in model.consultations)
        {
            if ((value.specialityId).ToString() == prevSpecialityId)
            {
                flag = 0;
                break;
            }
            else
            {
                prevSpecialityId = value.specialityId.ToString();
            }
        }
    
        return flag == 1;
    }

    public async Task<bool> CheckDethPatient(InspectionCreateModel model)
    {

        if (model.previousInspectionId != null)
        {
            var status = await _patientRepository.FindInspection(model.previousInspectionId);
    
            if (status.conclusion == Conclusion.Death)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public async Task<InspectionShortModel[]> GetInspectionWithoutChild(Guid patientId, string? partName)
    {
        List<InspectionShortModel> inspections = new List<InspectionShortModel>();
        Inspection[] allInspections = await _patientRepository.GetInspetionWithoutChild(patientId);

        foreach (var value in allInspections)
        {
            List<DiagnosisModel> diagnosis = await _patientRepository.GetDiagnosisInspectionWithUotChild(value.id, partName);

            foreach (var x in diagnosis)
            {
                InspectionShortModel shortInspection = new InspectionShortModel
                {
                    id = value.id,
                    createTime = value.createTime,
                    date = value.date,
                    diagnosis = x
                };
                inspections.Add(shortInspection);
            }
        }

        return inspections.ToArray();
    }
}