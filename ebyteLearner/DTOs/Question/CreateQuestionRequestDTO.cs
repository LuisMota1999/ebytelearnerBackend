using ebyteLearner.Models;
using System.ComponentModel.DataAnnotations;
using ebyteLearner.DTOs.Answer;
namespace ebyteLearner.DTOs.Question
{
    public class CreateQuestionRequestDTO
    {
        [Required]
        public int QuestionSlide { get; set; }
        [Required]
        public string QuestionName { get; set; }
        [Required]
        public List<AnswerDTO> QuestionAnswers { get; set; }
        [Required]
        public float QuestionScore { get; set; } = 0;
        [Required]
        public Guid PDFId { get; set; }
    }
}
