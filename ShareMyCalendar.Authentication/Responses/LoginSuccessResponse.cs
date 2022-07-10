using ShareMyCalendar.Authentication.Data;

namespace ShareMyCalendar.Authentication.Responses
{
    public record LoginSuccessResponse(User User, string AccessToken);
}
