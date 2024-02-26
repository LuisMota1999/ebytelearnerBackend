using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Services;

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

        [HttpGet("All")]
        public IActionResult GetAllCourses()
        {
            var response = _courseService.GetAllCourses().Result;
            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourse([FromRoute] Guid id)
        {

            var response = _courseService.GetCourse(id).Result;
            return Ok(response);

        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseRequestDTO request)
        {
            _courseService.UpdateCourse(id, request);
            return Ok($"Course {id} updated with success");
        }

        [HttpPost("Delete/{id}")]
        public IActionResult RemoveCourse([FromRoute] Guid id, [FromBody] Guid courseID)
        {
            _courseService.DeleteCourse(courseID);
            return Ok($"Course {id} deleted with success");

        }

        [HttpPost("Create")]
        public IActionResult CreateCourse([FromBody] CreateCourseRequestDTO request)
        {
            _courseService.CreateCourse(request);
            return Ok($"Course {request.CourseName} created with success");

        }

        [HttpPut("AssociateModule")]
        public IActionResult AssociateModule([FromBody] AssociateModuleRequest request)
        {
            _courseService.AssocModuleToCourse(request);
            return Ok($"Module {request.ModuleID} associated with success");

        }
    }
}
