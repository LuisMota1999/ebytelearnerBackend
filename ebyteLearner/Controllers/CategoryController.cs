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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieve all categories.
        /// </summary>
        /// <remarks>
        /// Retrieves all categories optionally limiting the number of returned rows.
        ///
        /// Sample request:
        ///
        ///     GET /All?returnRows=10
        /// </remarks>
        /// <param name="returnRows">Optional. The number of rows to return.</param>
        /// <returns>Returns the response containing all categories.</returns>
        [HttpGet("All")]
        public async Task<IActionResult> GetAllCategories([FromQuery] int returnRows = 0)
        {
            try
            {
                var response = await _categoryService.GetAllCategories(returnRows);
                return Ok(response);
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


        /// <summary>
        /// Get a category by its ID.
        /// </summary>
        /// <remarks>
        /// Retrieves a category from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>Returns the category with the specified ID.</returns>
        [HttpGet("{id}")]
        public IActionResult GetCategory([FromRoute] Guid id)
        {
            try
            {
                var response = _categoryService.GetCategory(id).Result;
                return Ok(response);
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


        /// <summary>
        /// Update a category by its ID.
        /// </summary>
        /// <remarks>
        /// Updates a category in the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the category to update.</param>
        /// <param name="request">The updated category information.</param>
        /// <returns>Returns a message indicating the success of the update operation.</returns>
        [HttpPut("Update/{id}")]
        public IActionResult UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDTO request)
        {
            try
            {
                _categoryService.UpdateCategory(id, request);
                return Ok($"Category {id} updated with success");
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


        /// <summary>
        /// Delete a category by its ID.
        /// </summary>
        /// <remarks>
        /// Deletes a category from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>Returns a message indicating the success of the delete operation.</returns>
        [HttpPost("Delete/{id}")]
        public IActionResult DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return Ok($"Category {id} deleted with success");
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


        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <remarks>
        /// Creates a new category in the database with the provided information.
        /// </remarks>
        /// <param name="request">The information of the category to create.</param>
        /// <returns>Returns a message indicating the success of the creation operation.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO request)
        {
            try
            {
                int rowsAffected = await _categoryService.CreateCategory(request);

                if (rowsAffected > 0)
                {
                    return Ok($"Category named {request.CategoryName} created successfully!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Failed to create category");
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
