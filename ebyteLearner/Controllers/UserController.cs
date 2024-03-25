using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.User;
using ebyteLearner.Models;
using ebyteLearner.Services;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <remarks>
        /// Retrieves all users from the database.
        /// </remarks>
        /// <returns>Returns a list of all users.</returns>
        [HttpGet("All")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var response = await _userService.GetAllUsers();
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
        /// Get all active teachers.
        /// </summary>
        /// <remarks>
        /// Retrieves all active teachers from the database.
        /// </remarks>
        /// <returns>Returns a list of all active teachers.</returns>
        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpGet("GetTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var response = await _userService.GetActiveTeacherUsers();
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
        /// Get a user by their ID.
        /// </summary>
        /// <remarks>
        /// Retrieves a user from the database based on their unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Returns the user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            try
            {
                var response = await _userService.GetUser(id);
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
        /// Search for users by query string.
        /// </summary>
        /// <remarks>
        /// Searches for usernames based on the provided query string.
        /// </remarks>
        /// <param name="q">The query string to search for.</param>
        /// <returns>Returns a list of users matching the search query.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> SearchUser([FromQuery] string q)
        {
            try
            {
                var response = await _userService.SearchUser(q);
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
        /// Update a user by their ID.
        /// </summary>
        /// <remarks>
        /// Updates a user in the database based on their unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="request">The updated user information.</param>
        /// <returns>Returns a message indicating the success of the update operation.</returns>
        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequestDTO request)
        {
            try
            {
                var response = await _userService.UpdateUser(id, request);
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
        /// Remove a user by their ID.
        /// </summary>
        /// <remarks>
        /// Removes a user from the database based on their unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the user to remove.</param>
        /// <returns>Returns a message indicating the success of the delete operation.</returns>
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> RemoveUser([FromRoute] Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok($"User {id} deleted successfully");
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