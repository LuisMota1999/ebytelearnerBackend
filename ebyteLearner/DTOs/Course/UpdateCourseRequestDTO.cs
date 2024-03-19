using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class UpdateCourseRequestDTO
    {
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public DateTimeOffset? UpdatedDate { get; init; } = DateTimeOffset.Now;
    }
}
