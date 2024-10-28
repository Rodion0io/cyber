using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class ConsultationCreateModel
{
    [Required]
    public Guid specialityId { get; set; }
    
    //Очень сомнительно
    public InspectionCommentCreateModel comment { get; set; }
}