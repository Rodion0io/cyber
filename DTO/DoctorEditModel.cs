using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class DoctorEditModel
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    public string name { get; set; }
    public DateTime? birthday { get; set; }
    [Required]
    public Gender gender { get; set; }
    public string? phone { get; set; }
}