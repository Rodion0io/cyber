namespace hospital_api.Modules;

public class PatientPagedListModel
{
    public PatientModel[] patients { get; set; }
    public PageInfoModel pagination { get; set; }
}