using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class DiagnosisModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    [Required]
    public string code { get; set; } //icd-10
    [Required]
    [MinLength(1)]
    public string name { get; set; }
    public string description { get; set; }
    [Required]
    public DiagnosisType type { get; set; }
}