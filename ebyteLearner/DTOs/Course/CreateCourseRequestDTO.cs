using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class CreateCourseRequestDTO
    {
        [Required]
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        [Required]
        public float CoursePrice { get; set; }
        [Required]
        public Guid CourseTeacherID { get; set; }
        public bool? IsPublished { get; set; } = false;
        public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.Now;
    }
}
