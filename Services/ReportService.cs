using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class ReportService : IReportService
{
    private readonly AccountsContext _context;

    public ReportService(AccountsContext context)
    {
        _context = context;
    }

    // public async Task<IcdRootsReportModel> GetReportModel()
    // {
    //     return new IcdRootsReportModel();
    // }
}