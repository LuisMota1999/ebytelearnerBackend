using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.DTOs.Auth;
using ebyteLearner.Services;

namespace ebyteLearner.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));        
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Auth([FromBody] AuthRequestDTO credentials)
        {

            var response = _authService.LoginCredentials(credentials);
            if (response == null)
                return Unauthorized();

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] RegisterRequestDTO request)
        {

            var response = _authService.RegisterUser(request);
            return Ok(response);

        }
    }
}
