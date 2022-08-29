namespace ShareMyCalendar.API.Responses;

public class SocialLoginSuccessResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? AccessToken { get; set; }
}
