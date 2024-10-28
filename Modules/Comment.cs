using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospital_api.Modules;

public class Comment
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public DateTime modifiedDate { get; set; }
    [MinLength(1)]
    public string content { get; set; }
    [Required]
    public Guid authorId { get; set; }
    [Required]
    public string author  { get; set; }
    public Guid parentId { get; set; }
    [ForeignKey("Consultation")]
    public Guid consultationId { get; set; }
    public Consultation Consultation { get; set; }
}