using System.ComponentModel.DataAnnotations;

namespace hospital_api.Modules;

public class LoginCredentialsModel
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    public string password { get; set; }
}