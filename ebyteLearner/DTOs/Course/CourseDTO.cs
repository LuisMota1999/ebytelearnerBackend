using ebyteLearner.DTOs.Module;

namespace ebyteLearner.DTOs.Course
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public List<ModuleDTO> Modules { get; set; }
    }
}
