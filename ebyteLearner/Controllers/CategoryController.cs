using ebyteLearner.DTOs.Category;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;   
            _categoryService = categoryService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllCategories([FromQuery] int returnRows = 0)
        {
            var response = await _categoryService.GetAllCategories(returnRows);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory([FromRoute] Guid id)
        {

            var response = _categoryService.GetCategory(id).Result;
            return Ok(response);

        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDTO request)
        {
            _categoryService.UpdateCategory(id, request);
            return Ok($"Category {id} updated with success");
        }

        [HttpPost("Delete/{id}")]
        public IActionResult DeleteCategory([FromRoute] Guid id, [FromBody] Guid courseID)
        {
            _categoryService.DeleteCategory(courseID);
            return Ok($"Category {id} deleted with success");

        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO request)
        {
            try
            {
                int rowsAffected = await _categoryService.CreateCategory(request);

                if (rowsAffected > 0)
                {
                    return Ok($"Category named ${request.CategoryName} with success!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create category");
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred {ex.Message}");
            }
        }
    }
}   
