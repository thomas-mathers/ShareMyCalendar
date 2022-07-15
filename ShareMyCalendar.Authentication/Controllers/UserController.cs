using Microsoft.AspNetCore.Mvc;
using ShareMyCalendar.Authentication.Models;
using ShareMyCalendar.Authentication.Requests;
using ThomasMathers.Common.IAM.Data;
using ThomasMathers.Common.IAM.Services;

namespace ShareMyCalendar.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IAuthService authService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest body)
        {
            var user = new User { UserName = body.UserName, Email = body.Email };
            var response = await _userService.Register(user, body.Password);

            return response.Match<IActionResult>(x => BadRequest(x.Errors), Ok);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            var response = await _authService.Login(body.UserName, body.Password);

            return response.Match<IActionResult>(
                NotFound,
                x => new ObjectResult(Constants.Errors.UserLockedOut) { StatusCode = 403 },
                x => new ObjectResult(Constants.Errors.LoginRequiresTwoFactor) { StatusCode = 403 },
                x => new ObjectResult(Constants.Errors.LoginIsNotAllowed) { StatusCode = 403 },
                x => Unauthorized(Constants.Errors.IncorrectPassword),
                Ok
            );
        }

        [HttpPut("{username}/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(string username, [FromBody] ChangePasswordRequest body)
        {
            var changePasswordResponse = await _authService.ChangePassword(username, body.CurrentPassword, body.Token, body.NewPassword);

            return changePasswordResponse.Match<IActionResult>(
                NotFound,
                x => BadRequest(x.Errors),
                Ok
            );
        }

        [HttpPost("{username}/password/resets")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GeneratePasswordResetToken(string username)
        {
            var user = await _userService.GetUserByUserName(username);

            if (user == null)
            {
                return NotFound();
            }

            var token = await _authService.GeneratePasswordResetToken(user);

            return Ok(token);
        }
    }
}