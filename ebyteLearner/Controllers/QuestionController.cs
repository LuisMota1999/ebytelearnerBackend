using ebyteLearner.DTOs.Question;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ebyteLearner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }

       
        [HttpGet("{id}")]
        public IActionResult GetQuestion([FromRoute] Guid id)
        {

            var response = _questionService.GetQuestion(id).Result;
            return Ok(response);

        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequestDTO request)
        {
            _questionService.UpdateQuestion(id, request);
            return Ok($"Question {id} updated with success");
        }

        [HttpPost("Delete/{id}")]
        public IActionResult RemoveQuestion([FromRoute] Guid id, [FromBody] Guid questionID)
        {
            _questionService.DeleteQuestion(questionID);
            return Ok($"Question {id} deleted with success");

        }

        [HttpPost("Create")]
        public IActionResult CreateQuestion([FromBody] CreateQuestionRequestDTO request)
        {
            var response =  _questionService.CreateQuestion(request);
            return Ok(response);

        }

      
    }

}