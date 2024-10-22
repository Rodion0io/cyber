namespace hospital_api.Modules;

public class Icd10Model
{
    public Guid id { get; set; }
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public string code { get; set; }
    public string name { get; set; }
    public Guid parentId { get; set; }
    public int actual  { get; set; }
    public DateTime date { get; set; }
}