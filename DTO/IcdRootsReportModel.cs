namespace hospital_api.Modules;

public class IcdRootsReportModel
{
    public IcdRootsReportFiltersModel filters { get; set; }
    public IcdRootsReportRecordModel[] records { get; set; }
    public Dictionary<string, int> summaryByRoot { get; set; }
}