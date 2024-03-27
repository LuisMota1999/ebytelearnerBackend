using ebyteLearner.DTOs.GoogleDrive;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Create a folder in Google Drive.
        /// </summary>
        /// <remarks>
        /// Creates a folder in the specified location on Google Drive.
        /// </remarks>
        /// <param name="request">The request containing the folder name.</param>
        /// <param name="id">The ID of the parent folder where the new folder will be created.</param>
        /// <returns>Returns the information about the created folder.</returns>
        [AllowAnonymous]
        [HttpPost("{id}/CreateFolder")]
        public IActionResult CreateFolder([FromBody] GoogleDriveCreateFolderRequestDTO request, [FromRoute] string id)
        {
            try
            {
                var response = _driveService.CreateFolder(request.FolderName, id);
                return Ok(response);
            }
            catch (ValidationException v)
            {
                return StatusCode(500, $"A validation error occurred: {v.Message}");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }


        /// <summary>
        /// Grant permission for a user on a specific folder in Google Drive.
        /// </summary>
        /// <remarks>
        /// Grants the specified user a specific role (permission level) on the specified folder in Google Drive.
        /// </remarks>
        /// <param name="id">The ID of the folder on which permission is to be granted.</param>
        /// <param name="request">The request containing the email address of the user and the role to be granted.</param>
        /// <returns>Returns the response indicating the status of the permission grant operation.</returns>
        [AllowAnonymous]
        [HttpPost("{id}/GrantFolderPermission")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Replace YourResponseTypeHere with the actual response type
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GrantFolderPermission([FromBody] GoogleDriveGrantFolderPermissionRequest request, [FromRoute] string id)
        {
            try
            {
                var response = _driveService.GrantFolderPermission(request.EmailAddress, request.UserRole, id);
                return Ok(response);
            }
            catch (ValidationException v)
            {
                return StatusCode(500, $"A validation error occurred: {v.Message}");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }


        /// <summary>
        /// Get files from a specific folder in Google Drive.
        /// </summary>
        /// <remarks>
        /// Retrieves the list of files stored in the specified folder on Google Drive.
        /// </remarks>
        /// <returns>Returns the list of files in the folder.</returns>
        [AllowAnonymous]
        [HttpGet("GetFilesFromFolder")]
        public IActionResult GetFiles()
        {
            try
            {
                var response = _driveService.GetFilesFromFolder();
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        /// <summary>
        /// Get all folders from Google Drive.
        /// </summary>
        /// <remarks>
        /// Retrieves all folders stored in Google Drive.
        /// </remarks>
        /// <returns>Returns the list of all folders in Google Drive.</returns>
        [AllowAnonymous]
        [HttpGet("GetFolders")]
        public IActionResult GetFolders()
        {
            try
            {
                var response = _driveService.GetFolders();
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        /// <summary>
        /// Download a file from Google Drive.
        /// </summary>
        /// <remarks>
        /// Downloads the specified file from Google Drive.
        /// </remarks>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <returns>Returns the downloaded file.</returns>
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
