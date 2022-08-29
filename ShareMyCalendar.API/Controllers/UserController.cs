using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareMyCalendar.API.Mappers;
using ShareMyCalendar.API.Models;
using ShareMyCalendar.API.Requests;

using ThomasMathers.Infrastructure.IAM.Data.EF;
using ThomasMathers.Infrastructure.IAM.Services;
using ThomasMathers.Infrastructure.IAM.Social;

namespace ShareMyCalendar.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ISocialAuthService _socialAuthService;

    public UserController
    (
        ILogger<UserController> logger,
        IUserService userService,
        IAuthService authService,
        ISocialAuthService socialAuthService
    )
    {
        _logger = logger;
        _userService = userService;
        _authService = authService;
        _socialAuthService = socialAuthService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest body)
    {
        var user = new User { UserName = body.UserName, Email = body.Email };
        var response = await _userService.Register(user, Roles.User.Name, body.Password);

        return response.Match<IActionResult>(
            x => BadRequest(x.Errors),
            NotFound,
            x => Created($"user/{x.User.Id}", RegisterSuccessResponseMapper.Map(x))
        );
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);

        return user == null ? NotFound() : Ok(UserResponseMapper.Map(user));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();

        return Ok(users.Select(UserResponseMapper.Map));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        await _userService.DeleteUser(user);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAllUsers()
    {
        var users = await _userService.GetAllUsers();

        foreach (var u in users)
        {
            await _userService.DeleteUser(u);
        }

        return Ok();
    }

    [HttpPost("social-login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest body)
    {
        var response = await _socialAuthService.Login(body.Provider, body.UserId, body.AccessToken, Roles.User.Name);

        return response.Match<IActionResult>(
            NotFound,
            x => Ok(SocialLoginSuccessResponseMapper.Map(x))
        );
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
            x => Ok(LoginSuccessResponseMapper.Map(x))
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
    public async Task<IActionResult> ResetPassword(string username)
    {
        var resetPasswordResponse = await _authService.ResetPassword(username);

        return resetPasswordResponse.Match<IActionResult>(
            NotFound,
            Ok);
    }
}