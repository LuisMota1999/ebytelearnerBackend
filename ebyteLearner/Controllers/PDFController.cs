using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PDFController : ControllerBase
    {
        private readonly IPDFService _pdfService;
        private readonly ILogger<PDFController> _logger;

        public PDFController(ILogger<PDFController> logger, IPDFService pdfService)
        {
            _logger = logger;
            _pdfService = pdfService;
        }


        [HttpPost("{moduleId}/Upload")]
        public async Task<IActionResult> UploadPDF(IFormFile file, [FromRoute] Guid moduleId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or file is empty");

            // Ensure the uploaded file is a PDF
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF files are allowed");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Convert the file contents to base64
                var base64Content = Convert.ToBase64String(memoryStream.ToArray());

                int numberOfPages = 0;
                // Open the PDF document
                using(var stream = file.OpenReadStream())
                using (var reader = new PdfReader(stream))
                {
                    numberOfPages = reader.NumberOfPages;
                }
                long contentLength = file.Length;
                // Pass the base64 content to your service method
                var result = await _pdfService.UploadPDF(base64Content, numberOfPages, file.FileName, moduleId, contentLength);

                if (result)
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
