using System.ComponentModel.DataAnnotations;
using Azure.Core.Pipeline;

namespace hospital_api.Modules;

public class ConsultationModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    public Guid inspectionId { get; set; }
    public SpecialityModel speciality { get; set; }
    public CommentModel[] comments { get; set; }
}