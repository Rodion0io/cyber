using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class PatientModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; } = DateTime.UtcNow;
    [Required]
    public string name { get; set; }
    public DateTime birthday { get; set; }
    [Required]
    public Gender gender { get; set; }
}