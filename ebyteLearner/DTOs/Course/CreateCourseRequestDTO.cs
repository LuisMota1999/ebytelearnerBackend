using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class CreateCourseRequestDTO
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public float CoursePrice { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CourseTeacherID { get; set; }
        public bool? IsPublished { get; set; } = false;
        public string? CourseDirectory { get; set; }
    }
}
