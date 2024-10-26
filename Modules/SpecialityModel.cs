namespace hospital_api.Modules;

public class SpecialityModel
{
    public string name { get; set; }
    
    // надо поменять string на Guid!!!
    public string id { get; set; }
    // public Guid id { get; set; }
    public DateTime createTime { get; set; }
}