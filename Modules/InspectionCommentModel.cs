using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospital_api.Modules;

public class InspectionCommentModel
{
    [Required]
    public Guid id { get; set; }
    [Required]
    public DateTime createTime { get; set; }
    public Guid parentId { get; set; }
    public string content { get; set; }
    [ForeignKey("DoctorModel")]
    public Guid author { get; set; } //Пока что string, потом надо поменять в Guid
    public DoctorModel DoctorModel { get; set; }
    public DateTime modifyTime { get; set; }
}