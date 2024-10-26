using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class InspectionCreateModel
{
    [Required]
    public DateTime date { get; set; } // видимо здесь придется делать проверку, чтобы время не было ранше, чем время поля createTime в модели InspectionModel
    [Required]
    [MinLength(1)]
    [MaxLength(5000)]
    public string anamnesis { get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(5000)]
    public string complaints { get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(5000)]
    public string treatment { get; set; }
    [Required]
    public Conclusion conclusion { get; set; }
    public DateTime nextVisitDate { get; set; } // зависит от Disease conclusion
    public DateTime deathDate { get; set; }
    public Guid previosInspectionId { get; set; }
    [Required]
    [MinLength(1)]
    public DiagnosisCreateModel diagnosis { get; set; }
    public ConsultationCreateModel consultations { get; set; }
}