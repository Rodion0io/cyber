using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IInspectionService
{
    public Task<InspectionModel> GetInspection(Guid id, Guid doctorId);
}