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

        /// <summary>
        /// Authenticate with username and password.
        /// </summary>
        /// <remarks>
        /// Authenticates a user with the provided username and password.
        /// All parameters in the request body are required.
        ///
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "username": "exampleUser",
        ///        "password": "examplePassword"
        ///     }
        /// </remarks>
        /// <param name="credentials">The authentication request containing username and password.</param>
        /// <returns>Returns the response indicating the success of the authentication along with a JWT token.</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Auth([FromBody] AuthRequestDTO credentials)
        {
            var response = _authService.LoginCredentials(credentials);
            if (response == null)
                return Unauthorized();

            return Ok(response);
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <remarks>
        /// Registers a new user with the provided information.
        /// All parameters in the request body are required.
        ///
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        "username": "exampleUser",
        ///        "email": "example@example.com",
        ///        "password": "examplePassword"
        ///     }
        /// </remarks>
        /// <param name="request">The registration request containing username, email and password.</param>
        /// <returns>Returns the response indicating the success of the registration.</returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] RegisterRequestDTO request)
        {
            var response = _authService.RegisterUser(request);
            return Ok(response);
        }
    }
}
