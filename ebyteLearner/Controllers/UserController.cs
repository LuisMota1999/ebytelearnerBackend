using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.User;
using ebyteLearner.Models;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("All")]
        public IActionResult GetAllUsers()
        {
            var response = _userService.GetAllUsers().Result;
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpGet("GetTeachers")]
        public IActionResult GetAllTeachers()
        {
            var response = _userService.GetActiveTeacherUsers().Result;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] Guid id)
        {

            var response = _userService.GetUser(id).Result;
            return Ok(response);

        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> SearchUser([FromQuery] string searchQuery)
        {
            var response = await _userService.SearchUser(searchQuery);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpPut("Update/{id}")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequestDTO request)
        {
            _userService.UpdateUser(id, request);
            return Ok($"User {id} updated with success");

        }

        [HttpPost("Delete/{id}")]
        public IActionResult RemoveUser([FromRoute] Guid id, [FromBody] Guid userID)
        {
            _userService.DeleteUser(userID);
            return Ok($"User {id} deleted with success");

        }

    }
}