namespace hospital_api.Modules;

public class IcdRootsReportFiltersModel
{
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string[] icdRoots { get; set; }
}