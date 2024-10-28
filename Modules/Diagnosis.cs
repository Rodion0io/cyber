using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class Diagnosis
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    [Required]
    [MinLength(1)]
    public string code { get; set; }
    [Required]
    [MinLength(1)]
    public string name { get; set; }
    [MaxLength(5000)]
    public string description { get; set; }
    [Required]
    public DiagnosisType type { get; set; }
    [Required]
    public Guid icdDiagnosisId { get; set; }
    [Required]
    public Guid inspectionId { get; set; }
}