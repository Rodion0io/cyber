using hospital_api.Enums;

namespace hospital_api.Modules;

public class IcdRootsReportRecordModel
{
    public string patientName { get; set; }
    public DateTime patientBirthdate { get; set; }
    public Gender gender { get; set; }
    public Dictionary<string, int> visitsByRoot { get; set; }
}