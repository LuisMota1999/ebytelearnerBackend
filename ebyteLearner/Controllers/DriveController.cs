using ebyteLearner.DTOs.GoogleDrive;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ebyteLearner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DriveController : ControllerBase
    {

        private readonly IDriveServiceHelper _driveService;
        private readonly ILogger<DriveController> _logger;

        public DriveController(ILogger<DriveController> logger, IDriveServiceHelper driveService)
        {
            _logger = logger;
            _driveService = driveService ?? throw new ArgumentNullException(nameof(driveService));

        }

        [AllowAnonymous]
        [HttpPost("CreateFolder")]
        public IActionResult CreateFolder([FromBody] GoogleDriveCreateFolderRequestDTO request)
        {
            if (request.FolderName != null)
            {
                string response = _driveService.CreateFolder("1m7tjucJGeEAbAHyvoCRhQdaoillkSpJn", request.FolderName);
                return Ok(response);
            }
            return BadRequest(new { message = "Something went wrong!" });
        }


        [AllowAnonymous]
        [HttpGet("GetFiles")]
        public IActionResult GetFiles()
        {
            var response = _driveService.GetFiles("1m7tjucJGeEAbAHyvoCRhQdaoillkSpJn");

            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("{fileId}")]
        public IActionResult DownloadFile(string fileId)
        {
            try
            {
                var response = _driveService.DriveDownloadFile(fileId);

                if (response == null)
                {
                    return NotFound();
                }

                var (stream, name) = response.Value;

                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/octet-stream", name);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
    }
}
