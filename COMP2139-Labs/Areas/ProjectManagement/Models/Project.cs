using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace COMP2139_Labs.Areas.ProjectManagement.Models
{
    public class Project
    {
        // Modelling our project database


        [Key]
        public int ProjectId { get; set; }


        [Required] // validate to make sure it's not empty
        [Display(Name = "Project Name")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public string Name { get; set; }


        // nullable with "?" symbol
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }


        [Display(Name = "Start Date")]
        [DataType(DataType.Date)] // annotation to specify the format and data type, validating the data to be for date
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Display(Name = "End Date")]
        [DataType(DataType.Date)] // annotation to specify the format and data type, validating the data to be for date
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        // nullable with "?" symbol
        public string? Status { get; set; }

        public List<ProjectTask>? Tasks { get; set; }
    }
}