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
        public string? ModuleDescription { get; set; } = null;
        public int? ModuleOrder { get; set; }

        [ForeignKey("ModulePDFId")]
        public Pdf Pdf { get; set; }
        public Guid ModulePDFId { get; set; }
        public bool? isPublished { get; set; } = false;
        public bool? isFree { get; set; } = false;
        public List<Session> Sessions { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [JsonIgnore]
        public Guid CourseId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDate { get; init; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedDate { get; init; }
    }
}