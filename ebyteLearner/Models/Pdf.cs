using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ebyteLearner.Models
{
    public class Pdf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string PDFName { get; set; }
        public int? PDFNumberPages { get; set; }
        public string? PDFContent { get; set; }
        public long PDFLength{ get; set; }
        public string? PDFPath { get; set; }

        [ForeignKey("ModuleID")]
        public Module Module { get; set; }
        [JsonIgnore]
        public Guid ModuleID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDate { get; init; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedDate { get; init; }
    }
}