namespace hospital_api.Modules;

public class Icd10SearchModel
{
    public List<Icd10RecordModel> records { get; set; }
    public PageInfoModel pagintaion { get; set; }
}