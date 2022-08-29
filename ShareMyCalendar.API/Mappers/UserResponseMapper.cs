using ShareMyCalendar.API.Responses;

using ThomasMathers.Infrastructure.IAM.Data.EF;

namespace ShareMyCalendar.API.Mappers;

public static class UserResponseMapper
{
    public static UserResponse Map(User user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        EmailConfirmed = user.EmailConfirmed,
        PhoneNumber = user.PhoneNumber,
        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
    };
}
