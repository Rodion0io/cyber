using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class InspectionModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    public DateTime date { get; set; }
    public string anamnesis { get; set; }
    public string complaints { get; set; }
    public string treatment { get; set; }
    public Conclusion conclusion { get; set; }
    public DateTime? nextVisitDate { get; set; }
    public DateTime? deathDate { get; set; }
    public Guid? baseInspectionId { get; set; }
    public Guid? previousInspectionId { get; set; }
    public PatientModel patient { get; set; }
    public DoctorModel doctor { get; set; }
    public DiagnosisModel[] diagnoses  { get; set; }
    public InspectionConsultationModel[] consultations { get; set; }
}