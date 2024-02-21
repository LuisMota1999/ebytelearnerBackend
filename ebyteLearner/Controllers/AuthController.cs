﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ebyteLearner.Data.Repository;
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
        [HttpGet("Teste")]
        public IActionResult Teste()
        {
            return Ok(new { message = "Teste" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Auth([FromBody] AuthRequestDTO credentials)
        {
            var response = _authService.LoginCredentials(credentials);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] RegisterRequestDTO request)
        {

            _authService.RegisterUser(request);
            return Ok(new { message = "Registration successful" });

        }
    }
}
