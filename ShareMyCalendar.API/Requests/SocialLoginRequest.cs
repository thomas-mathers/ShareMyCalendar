namespace ShareMyCalendar.API.Requests;

public class SocialLoginRequest
{
    public string Provider { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
