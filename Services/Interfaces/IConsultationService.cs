using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IConsultationService
{
    public Task<ConsultationModel> GetConcreteConsultation(Guid id);
}