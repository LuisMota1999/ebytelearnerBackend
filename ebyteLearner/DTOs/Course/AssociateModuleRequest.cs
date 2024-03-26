using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Course
{
    public class AssociateModuleRequest
    {
        [Required]
        public Guid CourseID { get; set; }
        [Required]
        public Guid ModuleID { get; set; }
    }
}
