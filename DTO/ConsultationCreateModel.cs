using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class ConsultationCreateModel
{
    [Required]
    public Guid specialityId { get; set; }
    [Required]
    public InspectionCommentCreateModel comment { get; set; }
}