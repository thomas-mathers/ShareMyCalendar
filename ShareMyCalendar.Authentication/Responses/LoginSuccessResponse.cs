namespace ShareMyCalendar.Authentication.Responses
{
    public record LoginSuccessResponse(Guid UserId, string UserName, string UserEmail, string AccessToken);
}
