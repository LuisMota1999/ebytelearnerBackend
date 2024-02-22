using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class UpdateCourseRequestDTO
    {
        [Required]
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public DateTimeOffset UpdatedDate { get; init; } = DateTimeOffset.Now;
    }
}
