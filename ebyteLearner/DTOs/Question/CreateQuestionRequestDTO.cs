using ebyteLearner.Models;
using System.ComponentModel.DataAnnotations;
using ebyteLearner.DTOs.Answer;
namespace ebyteLearner.DTOs.Question
{
    public class CreateQuestionRequestDTO
    {
        [Required]
        public int Slide { get; set; }
        [Required]
        public string QuestionName { get; set; }
        [Required]
        public List<AnswerDTO> Answers { get; set; }
        [Required]
        public float Score { get; set; }
        [Required]
        public Guid PDFId { get; set; }
        public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
