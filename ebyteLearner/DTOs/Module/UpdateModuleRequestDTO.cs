using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Module
{
    public class UpdateModuleRequestDTO
    {
        public string? ModuleName { get; set; }
        public string? ModuleDescription { get; set; }
        public string? CourseId { get; set; }
        public bool? isFree { get; set; }
        public Guid ModulePDFId { get; set; }
    }
}
