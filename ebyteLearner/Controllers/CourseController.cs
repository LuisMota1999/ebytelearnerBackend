using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Services;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ILogger<CourseController> logger, ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <remarks>
        /// Retrieves all courses from the database.
        /// </remarks>
        /// <param name="returnRows">The number of rows to return (optional).</param>
        /// <returns>Returns a list of all courses.</returns>
        [HttpGet("All")]
        public async Task<IActionResult> GetAllCourses([FromQuery] int returnRows = 0)
        {
            try
            {
                var response = await _courseService.GetAllCourses(returnRows);
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
        /// Get a course by its ID.
        /// </summary>
        /// <remarks>
        /// Retrieves a course from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>Returns the course with the specified ID.</returns>
        [HttpGet("{id}")]
        public IActionResult GetCourse([FromRoute] Guid id)
        {
            try
            {
                var response = _courseService.GetCourse(id).Result;
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
        /// Update a course by its ID.
        /// </summary>
        /// <remarks>
        /// Updates a course in the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the course to update.</param>
        /// <param name="request">The updated course information.</param>
        /// <returns>Returns a message indicating the success of the update operation.</returns>
        [HttpPut("Update/{id}")]
        public IActionResult UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseRequestDTO request)
        {
            try
            {
                _courseService.UpdateCourse(id, request);
                return Ok($"Course {id} updated successfully");
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
        /// Remove a course by its ID.
        /// </summary>
        /// <remarks>
        /// Removes a course from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the course to remove.</param>
        /// <returns>Returns a message indicating the success of the delete operation.</returns>
        [HttpPost("Delete/{id}")]
        public IActionResult RemoveCourse([FromRoute] Guid id)
        {
            try
            {
                _courseService.DeleteCourse(id);
                return Ok($"Course {id} deleted successfully");
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
        /// Create a new course.
        /// </summary>
        /// <remarks>
        /// Creates a new course in the database with the provided information.
        /// </remarks>
        /// <param name="request">The information of the course to create.</param>
        /// <returns>Returns a message indicating the success of the creation operation.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequestDTO request)
        {
            try
            {
                int rowsAffected = await _courseService.CreateCourse(request);

                if (rowsAffected > 0)
                {
                    return Ok($"Course named {request.CourseName} created successfully!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Failed to create course");
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


        /// <summary>
        /// Associate a module with a course.
        /// </summary>
        /// <remarks>
        /// Associates a module with a course in the database.
        /// </remarks>
        /// <param name="request">The request containing the module and course IDs.</param>
        /// <returns>Returns a message indicating the success of the association operation.</returns>
        [HttpPut("AssociateModule")]
        public IActionResult AssociateModule([FromBody] AssociateModuleRequest request)
        {
            try
            {
                _courseService.AssocModuleToCourse(request);
                return Ok($"Module {request.ModuleID} associated successfully");
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
