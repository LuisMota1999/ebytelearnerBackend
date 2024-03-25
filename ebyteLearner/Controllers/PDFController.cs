using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.Services;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PDFController : ControllerBase
    {
        private readonly IPDFService _pdfService;
        private readonly IDriveServiceHelper _driveService;
        private readonly ILogger<PDFController> _logger;

        public PDFController(ILogger<PDFController> logger, IPDFService pdfService, IDriveServiceHelper driveService)
        {
            _logger = logger;
            _pdfService = pdfService;
            _driveService = driveService;
        }


        /// <summary>
        /// Get PDF by module ID.
        /// </summary>
        /// <remarks>
        /// Retrieves the PDF associated with the specified module.
        /// </remarks>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>Returns the PDF associated with the specified module.</returns>
        [HttpGet("{moduleId}")]
        public async Task<IActionResult> GetPdfByModule([FromRoute] Guid moduleId)
        {
            try
            {
                var response = await _pdfService.GetPDFByModuleID(moduleId);
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
        /// Upload a PDF file to a module.
        /// </summary>
        /// <remarks>
        /// Uploads a PDF file to the specified module.
        /// </remarks>
        /// <param name="file">The PDF file to upload.</param>
        /// <param name="moduleId">The ID of the module to which the PDF file will be uploaded.</param>
        /// <returns>Returns a message indicating the success of the upload operation.</returns>
        [HttpPost("{moduleId}/Upload")]
        public async Task<IActionResult> UploadPDF(IFormFile file, [FromRoute] Guid moduleId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or file is empty");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF files are allowed");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Convert the file contents to base64
                var base64Content = Convert.ToBase64String(memoryStream.ToArray());

                int numberOfPages = 0;
                // Open the PDF document
                using (var stream = file.OpenReadStream())
                using (var reader = new PdfReader(stream))
                {
                    numberOfPages = reader.NumberOfPages;
                }
                long contentLength = file.Length;
                // Pass the base64 content to your service method
                var result = await _pdfService.UploadPDF(base64Content, numberOfPages, file.FileName, moduleId, contentLength, file.ContentType, memoryStream);

                if (result > 0)
                {
                    return Ok("PDF uploaded successfully");
                }
                else
                {
                    return StatusCode(500, "Failed to upload PDF");
                }
            }
        }


    }
}
