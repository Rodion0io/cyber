using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace hospital_api.Modules;

public class Icd10Model
{

    
    
    [Key]
    public Guid secondKey { get; set; }
    [Column("ID")]
    [JsonProperty("ID")]
    public int id { get; set; }

    [Column("REC_CODE")]
    [JsonProperty("REC_CODE")]
    public string? recCode { get; set; }

    public DateTime createTime { get; set; } = DateTime.UtcNow;

    [Column("MKB_CODE")]
    [JsonProperty("MKB_CODE")]
    public string code { get; set; }

    [Column("MKB_NAME")]
    [JsonProperty("MKB_NAME")]
    public string name { get; set; }

    [Column("ID_PARENT")]
    [JsonProperty("ID_PARENT")]
    public string? parentId { get; set; }

    [Column("ACTUAL")]
    [JsonProperty("ACTUAL")]
    public int? actual { get; set; }

    [Column("DATE")]
    [JsonProperty("DATE")]
    public string? date { get; set; }
}