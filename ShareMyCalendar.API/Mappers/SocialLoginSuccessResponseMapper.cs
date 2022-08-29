using ShareMyCalendar.API.Responses;

namespace ShareMyCalendar.API.Mappers;

public class SocialLoginSuccessResponseMapper
{
    public static SocialLoginSuccessResponse Map(ThomasMathers.Infrastructure.IAM.Social.Responses.SocialLoginSuccessResponse source) => new()
    {
        Id = source.User.Id,
        UserName = source.User.UserName,
        AccessToken = source.AccessToken
    };
}
