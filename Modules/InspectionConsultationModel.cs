using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospital_api.Modules;

public class InspectionConsultationModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public Guid inspectionId { get; set; }
    public SpecialityModel speciality { get; set; }
    [ForeignKey("DoctorModel")]
    public Guid rootComment { get; set; }
    public InspectionCommentModel InspectionCommentModel { get; set; }
    public int commentsNumber { get; set; }
}