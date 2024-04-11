using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Services;
using System.ComponentModel.DataAnnotations;
using ebyteLearner.DTOs.Module;
using iTextSharp.text.pdf;

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
        public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseRequestDTO request)
        {

            try
            {
                var result = await _courseService.UpdateCourse(id, request);
                int rowsAffected = result.rows;
                CourseDTO updatedCourseDTO = result.course;

                if (rowsAffected > 0)
                {
                    return Ok(updatedCourseDTO);
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Failed to create module");
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
        /// Upload a PDF file to a module.
        /// </summary>
        /// <remarks>
        /// Uploads a PDF file to the specified module.
        /// </remarks>
        /// <param name="file">The PDF file to upload.</param>
        /// <param name="courseId">The ID of the module to which the PDF file will be uploaded.</param>
        /// <returns>Returns a message indicating the success of the upload operation.</returns>
        [HttpPut("{courseId}/UploadCourseImage")]
        public async Task<IActionResult> UploadCourseImage(IFormFile file, [FromRoute] Guid courseId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or file is empty");

            if ((Path.GetExtension(file.FileName).ToLower() != ".png") && Path.GetExtension(file.FileName).ToLower() != ".jpeg" && Path.GetExtension(file.FileName).ToLower() != ".jpg")
                return BadRequest("Only PNG, JPEG or JPG files are allowed");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                long contentLength = file.Length;
                // Pass the base64 content to your service method
                var result = await _courseService.UploadCourseImage(memoryStream, courseId, file.FileName, file.ContentType);

                if (result.rows > 0)
                {
                    return Ok(result.course);
                }
                else
                {
                    return StatusCode(500, "Failed to upload PDF");
                }
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
        [HttpDelete("Delete/{id}")]
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
                var result = await _courseService.CreateCourse(request);
                int rowsAffected = result.rows;
                CourseDTO createdCourse = result.course;

                if (rowsAffected > 0)
                {
                    return Ok(createdCourse);
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
