using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ebyteLearner.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public int Slide { get; set; }
        public string QuestionName { get; set; }
        public List<Answer> Answers { get; set; }
        public float Score { get; set; }
        public Guid PDFId { get; set; }

        [ForeignKey("PDFId")]
        public Pdf RelatedPDF { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}