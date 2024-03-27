using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class UpdateCourseRequestDTO
    {
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public float? CoursePrice { get; set; }
        public string? CourseImageURL { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CourseTeacherID { get; set; }
    }
}
