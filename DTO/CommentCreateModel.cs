using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class CommentCreateModel
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string content { get; set; }
    public Guid parentId { get; set; }
}