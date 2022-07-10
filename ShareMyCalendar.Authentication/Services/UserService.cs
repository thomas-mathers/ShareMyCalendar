using Microsoft.AspNetCore.Identity;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Responses;

namespace ShareMyCalendar.Authentication.Services
{
    public interface IUserService
    {
        Task<RegisterResponse> Register(RegisterRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            var user = new User
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.UserName
            };

            var createResult = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!createResult.Succeeded)
            {
                return new IdentityErrorResponse(createResult.Errors);
            }

            return new RegisterSuccessResponse(user);
        }
    }
}
