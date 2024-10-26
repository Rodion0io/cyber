using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class InspectionConsultationModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public Guid inspectionId { get; set; }
    public SpecialityModel speciality { get; set; }
    // public InspectionCommentModel rootComment { get; set; } нужно добавить этот класс
    public int commentsNumber { get; set; }
}