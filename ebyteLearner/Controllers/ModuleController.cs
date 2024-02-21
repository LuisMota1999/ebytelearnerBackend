using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly ModuleService _moduleService;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(ILogger<ModuleController> logger, ModuleService moduleService)
        {
            _logger = logger;
            _moduleService = moduleService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetModule([FromRoute] Guid id)
        {

            var response = await _moduleService.GetModule(id);
            return Ok(response);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateModule([FromRoute] Guid id, [FromBody] UpdateModuleRequestDTO request)
        {
            await _moduleService.UpdateModule(id, request);
            return Ok($"Module {id} updated with success");
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> RemoveModule([FromRoute] Guid id, [FromBody] Guid moduleID)
        {
            await _moduleService.DeleteModule(moduleID);
            return Ok($"Module {id} deleted with success");

        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleRequestDTO request)
        {
            await _moduleService.CreateModule(request);
            return Ok($"Module {request.ModuleName} created with success");

        }

    }
}
