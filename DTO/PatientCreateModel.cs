using System.ComponentModel.DataAnnotations;
using hospital_api.Enums;

namespace hospital_api.Modules;

public class PatientCreateModel
{
    [Required]
    public string name { get; set; }
    public DateTime birthDate { get; set; }
    [Required]
    public Gender gender { get; set; }
}