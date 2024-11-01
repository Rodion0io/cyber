using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class InspectionShortModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    [Required]
    public DateTime date { get; set; }
    public DiagnosisModel diagnosis { get; set; }
}