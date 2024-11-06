namespace hospital_api.Modules;

public class InspectionPagedListModel
{
    public InspectionPreviewModel[] inspections { get; set; }
    public PageInfoModel pagination { get; set; }
}