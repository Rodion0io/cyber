using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IReportService
{
    public Task<IcdRootsReportModel> GetReportModel(DateTime start, DateTime end,
        List<Guid> icdRoot);
}