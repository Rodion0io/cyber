using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class DiagnosisCreateModel
{
    [Required]
    public Guid icdDiagnosisId { get; set; }
    [MaxLength(5000)]
    public string description { get; set; }
    [Required]
    public DiagnosisType type { get; set; }
}