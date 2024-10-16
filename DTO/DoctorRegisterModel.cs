using System.ComponentModel.DataAnnotations;
using hospital_api.CustomValidation;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class DoctorRegisterModel
{
    [Required]
    public string name { get; set; }
    [Required]
    public string password { get; set; }
    [Required]
    [EmailAddress]
    public string email { get; set; }
    public DateTime? birthday { get; set; }
    [Required]
    public Gender gender { get; set; }
    public string? phone { get; set; }
    [Required]
    [IsSpeciality]
    public Guid speciality { get; set; }
}