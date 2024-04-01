using ebyteLearner.DTOs.Category;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Models;

namespace ebyteLearner.DTOs.Course
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImageURL { get; set; }
        public string CourseDirectory { get; set; }
        public float CoursePrice { get; set; }
        public CategoryDTO CourseCategory { get; set; }
        public List<ModuleDTO> CourseModules { get; set; }
        public UserDTO CourseTeacher { get; set; }
        public bool IsPublished { get; set; }
       
    }
}
