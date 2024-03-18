using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Category
{
    public class CreateCategoryRequestDTO
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
