using Microsoft.AspNetCore.Mvc;
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

        public UserController(ILogger<UserController> logger, IAuthService authService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest body)
        {
            var response = await _userService.Register(body);

            return response.Match<IActionResult>(
                x => StatusCode(400, ApiResponse.Failure(x)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest body)
        {
            var response = await _authService.ChangePassword(body);

            return response.Match<IActionResult>(
                x => StatusCode(404, ApiResponse.Failure(x)),
                x => StatusCode(400, ApiResponse.Failure(x)),
                x => StatusCode(200, ApiResponse.Success(x))
            );
        }
    }
}