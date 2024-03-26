using ebyteLearner.DTOs.Module;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Module
{
    public class CreateModuleRequestDTO
    {
        [Required]
        public string ModuleName { get; set; }
        public string? ModuleDescription { get; set; }
        [Required]
        public Guid CourseID { get; set; }
    }
}
