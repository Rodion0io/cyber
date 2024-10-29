using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospital_api.Modules;

public class Consultation
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    public Guid inspectionId { get; set; }
    [ForeignKey("SpecialityModel")]
    public Guid specialityId { get; set; }
    public virtual SpecialityModel SpecialityModel { get; set; }
}