using hospital_api.Modules;
using hospital_api.Repositories;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class InspectionService : IInspectionService
{

    private readonly IInspectionRepository _inspectionRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorServic _doctorServic;

    public InspectionService(IInspectionRepository inspectionRepository, IPatientRepository patientRepository,
        IDoctorServic doctorServic)
    {
        _inspectionRepository = inspectionRepository;
        _patientRepository = patientRepository;
        _doctorServic = doctorServic;
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
                doctor = _doctorServic.GetDoctorInfa(doctorId.ToString()),
                diagnoses = listDiagnosis,
                consultations = listConsultations
            };
            return result;
        }
    }
    
}