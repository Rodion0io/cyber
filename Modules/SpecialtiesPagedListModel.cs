namespace hospital_api.Modules;

public class SpecialtiesPagedListModel
{
    public List<SpecialityModel> specialties { get; set; }
    public PageInfoModel pagintaion { get; set; }
}