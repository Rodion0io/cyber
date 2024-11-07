using hospital_api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace hospital_api.Services.Interfaces;

public interface IInspectionService
{
    public Task<InspectionModel> GetInspection(Guid id, Guid doctorId);
    public Task EditInspection(Guid inspectionId, Guid doctorId, InspectionEditModel model);
    public Task<bool> CheckValidInspection(Guid inspectionId);
    public Task<List<InspectionPreviewModel>> GetInspectionChain(Guid rootInspectionId);
}