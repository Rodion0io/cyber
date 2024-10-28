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

    public async Task AddInpection(InspectionCreateModel model, Guid patintId, Guid doctorId, string doctorName)
    {

        Inspection newInspection = new Inspection
        {
            // надо добавить baseInspectionId
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

        foreach (var diagnosis in model.diagnosis)
        {
            Diagnosis newDiagnosis = new Diagnosis
            {
                code = await _dictionaryRepository.getIcd10Code((diagnosis.icdDiagnosisId).ToString()),
                name = await _dictionaryRepository.getIcd10Name((diagnosis.icdDiagnosisId).ToString()),
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
        
            Comment newComment = new Comment
            {
                content = comment.comment.content,
                authorId = doctorId,
                author = doctorName,
                consultationId = newConsultation.id
            };
            
            await _patientRepository.AddConsultation(newConsultation);
            await _patientRepository.AddComments(newComment);
        }

        await _patientRepository.AddInspection(newInspection);

        
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
    
    public bool CheckConclusion(InspectionCreateModel model)
    {
        if ((model.conclusion == Conclusion.Disease && model.nextVisitDate != null && model.deathDate == null) ||
            (model.conclusion == Conclusion.Death && model.nextVisitDate == null && model.deathDate != null) ||
            (model.conclusion == Conclusion.Recovery && model.nextVisitDate == null && model.deathDate == null))
        {
            return true;
        }
        else
        {
            return false;
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
}