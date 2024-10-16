using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class Doctor
{
    [Required]
    public Guid id { get; set; }
    
    [Required]
    public DateTime createTime { get; set; }
    [Required]
    public string name { get; set; }
    public DateTime? birthday { get; set; }
    [Required]
    public Gender gender { get; set; }
    [Required]
    [EmailAddress]
    public string email { get; set; }
    public string? phone { get; set; }
    [Required]
    public Guid speciality { get; set; }
    [Required]
    public string password { get; set; }
}