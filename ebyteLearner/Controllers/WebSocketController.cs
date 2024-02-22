using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ebyteLearner.DTOs.WebSocket;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly ILogger<WebSocketController> _logger;
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(ILogger<WebSocketController> logger, IWebSocketService webSocketService)
        {
            _logger = logger;
            _webSocketService = webSocketService;
        }

        [HttpGet("{UsernameID}")]
        public async Task<IActionResult> ConnectWebSocket([FromRoute] string UsernameID)
        {
            var context = ControllerContext.HttpContext;
            await _webSocketService.InitConnectionSocket(context);
            return new EmptyResult();
        }

        [HttpPost("Send/{UsernameID}")]
        public IActionResult SendMessage([FromRoute] string UsernameID, [FromBody] MessageDTO messageDto)
        {
            _webSocketService.Sender(UsernameID, messageDto.Message);
            return Ok();
        }

        [HttpPost("Sendall/{UsernameID}")]
        public IActionResult SendToAll([FromRoute] string UsernameID, [FromBody] MessageDTO messageDto)
        {
            _webSocketService.SenderAll(UsernameID, messageDto.Message);
            return Ok();
        }
    }
}
