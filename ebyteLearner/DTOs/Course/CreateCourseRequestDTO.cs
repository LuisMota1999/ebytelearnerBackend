using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class CreateCourseRequestDTO
    {
        [Required]
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public float CoursePrice { get; set; }
        public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.Now;
    }
}
