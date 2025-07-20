using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class CreateCourseForm
    {
        [Required]
        public string CourseName {  get; set; }
        [Required]
        public string TeacherName { get; set; }
    }
}
