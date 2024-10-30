using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class InspectionConsultationModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    public Guid inspectionId { get; set; }
    public SpecialityModel speciality { get; set; }
    public Comment? rootComment { get; set; }
}