using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; init; }
        public string CategoryName { get; set; }

    }
}
