using hospital_api.Modules;

namespace hospital_api.Repositories.repositoryInterfaces;

public interface IInspectionRepository
{
    public Task<Inspection> GetInspection(Guid id);
    public Task<DiagnosisModel[]> GetDiagnosis(Guid inspectionId);
    public Task<InspectionConsultationModel[]> GetInspectionConsultations(Guid inspectionId);
}