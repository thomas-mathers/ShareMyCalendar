using ShareMyCalendar.API.Responses;
using ThomasMathers.Infrastructure.IAM.Data;

namespace ShareMyCalendar.API.Mappers
{
    public static class UserResponseMapper
    {
        public static UserResponse Map(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            };
        }
    }
}
