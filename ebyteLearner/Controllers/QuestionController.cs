using ebyteLearner.DTOs.Question;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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


        /// <summary>
        /// Get a question by its ID.
        /// </summary>
        /// <remarks>
        /// Retrieves a question from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the question.</param>
        /// <returns>Returns the question with the specified ID.</returns>
        [HttpGet("{id}")]
        public IActionResult GetQuestion([FromRoute] Guid id)
        {
            try
            {
                var response = _questionService.GetQuestion(id).Result;
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Update a question by its ID.
        /// </summary>
        /// <remarks>
        /// Updates a question in the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the question to update.</param>
        /// <param name="request">The updated question information.</param>
        /// <returns>Returns a message indicating the success of the update operation.</returns>
        [HttpPut("Update/{id}")]
        public IActionResult UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequestDTO request)
        {
            try
            {
                _questionService.UpdateQuestion(id, request);
                return Ok($"Question {id} updated successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove a question by its ID.
        /// </summary>
        /// <remarks>
        /// Removes a question from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the question to remove.</param>
        /// <param name="questionID">The ID of the question to delete.</param>
        /// <returns>Returns a message indicating the success of the delete operation.</returns>
        [HttpDelete("Delete/{id}")]
        public IActionResult RemoveQuestion([FromRoute] Guid id, [FromBody] Guid questionID)
        {
            try
            {
                _questionService.DeleteQuestion(questionID);
                return Ok($"Question {id} deleted successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new question.
        /// </summary>
        /// <remarks>
        /// Creates a new question in the database with the provided information.
        /// </remarks>
        /// <param name="request">The information of the question to create.</param>
        /// <returns>Returns a message indicating the success of the creation operation.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequestDTO request)
        {
            try
            {
                int rowsAffected = await _questionService.CreateQuestion(request);

                if (rowsAffected > 0)
                {
                    return Ok($"Question {request.QuestionName} created successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Failed to create question");
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}