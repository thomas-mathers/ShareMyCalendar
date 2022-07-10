using Microsoft.AspNetCore.Mvc;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Responses;
using ShareMyCalendar.Authentication.Services;

namespace ShareMyCalendar.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            var response = await _authService.Login(body);

            return response.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(x)),
                x => StatusCode(403, ApiResponse.Failure(x)),
                x => StatusCode(403, ApiResponse.Failure(x)),
                x => StatusCode(403, ApiResponse.Failure(x)),
                x => StatusCode(401, ApiResponse.Failure(x)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpGet("password-reset-tokens")]
        public async Task<IActionResult> GeneratePasswordResetToken([FromBody] GeneratePasswordResetTokenRequest body)
        {
            var response = await _authService.GeneratePasswordResetToken(body.UserName);

            return response.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(x)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpPut("passwords")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body)
        {
            var response = await _authService.ResetPassword(body);

            return response.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(x)),
                x => StatusCode(400, ApiResponse.Failure(x)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }
    }
}