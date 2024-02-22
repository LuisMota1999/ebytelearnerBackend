using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ebyteLearner.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public List<Session> Sessions { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [JsonIgnore]
        public Guid CourseId { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}