using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Services;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(ILogger<ModuleController> logger, IModuleService moduleService)
        {
            _logger = logger;
            _moduleService = moduleService;
        }


        /// <summary>
        /// Get a module by its ID.
        /// </summary>
        /// <remarks>
        /// Retrieves a module from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the module.</param>
        /// <returns>Returns the module with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModule([FromRoute] Guid id)
        {
            try
            {
                var response = await _moduleService.GetModule(id);
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
        /// Update a module by its ID.
        /// </summary>
        /// <remarks>
        /// Updates a module in the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the module to update.</param>
        /// <param name="request">The updated module information.</param>
        /// <returns>Returns a message indicating the success of the update operation.</returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateModule([FromRoute] Guid id, [FromBody] UpdateModuleRequestDTO request)
        {
            try
            {
                await _moduleService.UpdateModule(id, request);
                return Ok($"Module {id} updated successfully");
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
        /// Remove a module by its ID.
        /// </summary>
        /// <remarks>
        /// Removes a module from the database based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the module to remove.</param>
        /// <returns>Returns a message indicating the success of the delete operation.</returns>
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> RemoveModule([FromRoute] Guid id)
        {
            try
            {
                await _moduleService.DeleteModule(id);
                return Ok($"Module {id} deleted successfully");
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
        /// Create a new module.
        /// </summary>
        /// <remarks>
        /// Creates a new module in the database with the provided information.
        /// </remarks>
        /// <param name="request">The information of the module to create such as name, description and associated session.</param>
        /// <returns>Returns a message indicating the success of the creation operation.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleRequestDTO request)
        {
            try
            {
                int rowsAffected = await _moduleService.CreateModule(request);

                if (rowsAffected > 0)
                {
                    return Ok($"Module named {request.ModuleName} created successfully!");
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
    }
}
