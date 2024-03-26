using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public int QuestionSlide { get; set; }
        public string QuestionName { get; set; }
        public List<Answer> QuestionAnswers { get; set; }
        public float QuestionScore { get; set; } = 0;
        public Guid PDFId { get; set; }

        [ForeignKey("PDFId")]
        public Pdf RelatedPDF { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDate { get; init; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedDate { get; init; }
    }
}