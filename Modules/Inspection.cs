using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class Inspection
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public DateTime date { get; set; }
    public string anamnesis { get; set; }
    public string complaints { get; set; }
    public string treatment { get; set; }
    public Conclusion conclusion { get; set; }
    public DateTime nextVisitDate { get; set; }
    public DateTime deathDate { get; set; }
    public Guid baseInspectionId { get; set; }
    public Guid previousInspectionId { get; set; }
    [ForeignKey("PatientModel")]
    public Guid patient { get; set; }
    public PatientModel PatientModel { get; set; }
    [ForeignKey("Doctor")]
    public Guid doctor { get; set; }
    public Doctor Doctor { get; set; }
}