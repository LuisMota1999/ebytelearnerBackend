using ebyteLearner.DTOs.Category;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllCourses([FromQuery] int returnRows = 0)
        {
            var response = await _categoryService.GetAllCategories(returnRows);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourse([FromRoute] Guid id)
        {

            var response = _categoryService.GetCategory(id).Result;
            return Ok(response);

        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDTO request)
        {
            _categoryService.UpdateCategory(id, request);
            return Ok($"Category {id} updated with success");
        }

        [HttpPost("Delete/{id}")]
        public IActionResult RemoveCourse([FromRoute] Guid id, [FromBody] Guid courseID)
        {
            _categoryService.DeleteCategory(courseID);
            return Ok($"Category {id} deleted with success");

        }

        [HttpPost("Create")]
        public IActionResult CreateCourse([FromBody] CreateCategoryRequestDTO request)
        {
            _categoryService.CreateCategory(request);
            return Ok($"Category {request.CategoryName} created with success");

        }
    }
}   
