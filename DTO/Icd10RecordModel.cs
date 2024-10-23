namespace hospital_api.Modules;

public class Icd10RecordModel
{
    public int id { get; set; }
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public string code { get; set; }
    public string name { get; set; }
}