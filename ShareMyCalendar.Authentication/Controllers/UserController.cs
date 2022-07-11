using Microsoft.AspNetCore.Mvc;
using ShareMyCalendar.Authentication.Models;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Responses;
using ShareMyCalendar.Authentication.Services;

namespace ShareMyCalendar.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(ILogger<UserController> logger, IAuthService authService, IUserService userService, IEmailService emailService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest body)
        {
            var response = await _userService.Register(body);

            return response.Match<IActionResult>(
                x => StatusCode(400, ApiResponse.Failure(Constants.Errors.IdentityError)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            var response = await _authService.Login(body);

            return response.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(Constants.Errors.NotFound)),
                x => StatusCode(403, ApiResponse.Failure(Constants.Errors.UserLockedOut)),
                x => StatusCode(403, ApiResponse.Failure(Constants.Errors.LoginRequiresTwoFactor)),
                x => StatusCode(403, ApiResponse.Failure(Constants.Errors.LoginIsNotAllowed)),
                x => StatusCode(401, ApiResponse.Failure(Constants.Errors.LoginFailed)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest body)
        {
            var changePasswordResponse = await _authService.ChangePassword(body);

            return changePasswordResponse.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(Constants.Errors.NotFound)),
                x => StatusCode(400, ApiResponse.Failure(Constants.Errors.IdentityError)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpPost("password/resets")]
        public async Task<IActionResult> GeneratePasswordResetToken([FromBody] GeneratePasswordResetTokenRequest body)
        {
            var user = await _userService.GetUserByUserName(body.UserName);

            if (user == null)
            {
                return StatusCode(404, ApiResponse.Failure(Constants.Errors.NotFound));
            }

            var token = await _authService.GeneratePasswordResetToken(user);

            await _emailService.SendForgotPasswordEmail(user, token);

            return StatusCode(200, ApiResponse.Success());
        }
    }
}