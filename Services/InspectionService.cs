using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services;

public class InspectionService : IInspectionService
{

    private readonly IInspectionRepository _inspectionRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorServic _doctorServic;
    private readonly IDictionaryRepository _dictionaryRepository;

    public InspectionService(IInspectionRepository inspectionRepository, IPatientRepository patientRepository,
        IDoctorServic doctorServic, IDictionaryRepository dictionaryRepository)
    {
        _inspectionRepository = inspectionRepository;
        _patientRepository = patientRepository;
        _doctorServic = doctorServic;
        _dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<InspectionModel> GetInspection(Guid inspectionId, Guid doctorId)
    {
        var model = await _inspectionRepository.GetInspection(inspectionId);

        if (model == null)
        {
            return null;
        }
        else
        {
            DiagnosisModel[] listDiagnosis = await _inspectionRepository.GetDiagnosis(inspectionId);
            InspectionConsultationModel[] listConsultations = await _inspectionRepository.GetInspectionConsultations(inspectionId);
            
            InspectionModel result = new InspectionModel
            {
                id = model.id,
                createTime = model.createTime,
                date = model.date,
                anamnesis = model.anamnesis,
                complaints = model.complaints,
                treatment = model.treatment,
                conclusion = model.conclusion,
                nextVisitDate = model.nextVisitDate,
                deathDate = model.deathDate,
                baseInspectionId = model.baseInspectionId,
                previousInspectionId = model.previousInspectionId,
                patient = await _patientRepository.FindPatient((model.patient).ToString()),
                doctor = await _doctorServic.GetDoctorInfa(doctorId.ToString()),
                diagnoses = listDiagnosis,
                consultations = listConsultations
            };
            return result;
        }
    }
    
    //
    // "anamnesis": "string",
    // "complaints": "string",
    // "treatment": "string",
    // "conclusion": "Disease",
    // "nextVisitDate": "2024-10-31T05:19:46.173Z",
    // "deathDate": "2024-10-31T06:15:32.823Z",

    public async Task EditInspection(Guid inspectionId, Guid doctorId, InspectionEditModel model)
    {
        var inspectionEditModel = await _inspectionRepository.GetInspection(inspectionId);

        await _inspectionRepository.ClearDiagnosis(inspectionId);

        if (inspectionEditModel == null)
        {
            throw new Exception("Осмотра с таким Id нет!");
        }
        else if (inspectionEditModel.doctor != doctorId)
        {
            throw new Exception("Этот доктор не может редактировать чужой осмотр!");
        }
        else
        {
            
            int countMainTypes = model.diagnosis.Count(i => i.type == DiagnosisType.Main);
            if (countMainTypes != 1)
            {
                throw new Exception("Должен быть один диагноз с типом Main");
            }
            else
            {
                if (model.anamnesis != inspectionEditModel.anamnesis)
                {
                    inspectionEditModel.anamnesis = model.anamnesis;
                }

                if (model.complaints != inspectionEditModel.complaints)
                {
                    inspectionEditModel.complaints = model.complaints;
                }

                if (model.treatment != inspectionEditModel.treatment)
                {
                    inspectionEditModel.treatment = model.treatment;
                }

                if (model.conclusion != inspectionEditModel.conclusion)
                {
                    inspectionEditModel.conclusion = model.conclusion;
                }

                if (model.nextVisitDate != inspectionEditModel.nextVisitDate &&
                    model.conclusion == Conclusion.Disease  &&
                    model.nextVisitDate > inspectionEditModel.date)
                {
                    inspectionEditModel.nextVisitDate = model.nextVisitDate;
                }

                if (model.deathDate != inspectionEditModel.deathDate &&
                    model.conclusion == Conclusion.Death)
                {
                    inspectionEditModel.deathDate = model.deathDate;
                }

                await _inspectionRepository.SaveChangesInspection(inspectionEditModel);
                
                foreach (var value in model.diagnosis)
                {
                    Diagnosis newDiagnosis = new Diagnosis
                    {
                        code = await _dictionaryRepository.getIcd10Code((value.icdDiagnosisId)),
                        name = await _dictionaryRepository.getIcd10Name((value.icdDiagnosisId)),
                        description = value.description,
                        type = value.type,
                        icdDiagnosisId = value.icdDiagnosisId,
                        inspectionId = inspectionId
                    };
                    
                    await _patientRepository.AddDiagnosis(newDiagnosis);
                }
            }
        }
    }
    
}