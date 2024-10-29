using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace hospital_api.Modules;

public class Icd10RecordModel
{
    public Guid secondKey { get; set; }
    public int id { get; set; }
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public string code { get; set; }
    public string name { get; set; }
}