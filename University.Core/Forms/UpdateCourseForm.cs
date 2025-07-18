using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class UpdateCourseForm
    {
        [Required]
        public string CourseName {  get; set; } 
    }
}
