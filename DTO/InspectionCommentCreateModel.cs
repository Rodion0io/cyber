using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class InspectionCommentCreateModel
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string content { get; set; }
}