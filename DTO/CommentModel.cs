using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class CommentModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    public DateTime modifiedDate { get; set; }
    [Required]
    [MinLength(1)]
    public string content { get; set; }
    [Required]
    public Guid authorId { get; set; }
    [Required]
    [MinLength(1)]
    public string author { get; set; }
    public Guid? parentId { get; set; }
}