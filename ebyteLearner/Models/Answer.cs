using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string AnswerResponse { get; set; }
        public float AnswerScore { get; set; }
        public bool AnswerCorrect { get; set; }

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }
        public Guid QuestionID { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}