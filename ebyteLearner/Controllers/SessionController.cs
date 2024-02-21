using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Models;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger, ISessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpPost("Create")]
        public IActionResult CreateCourse([FromBody] CreateSessionRequestDTO request)
        {
            _sessionService.CreateSession(request);
            return Ok($"Session {request.SessionName} created with success");
        }
    }       
}
