namespace ShareMyCalendar.API.Requests;

public record LoginRequest
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
