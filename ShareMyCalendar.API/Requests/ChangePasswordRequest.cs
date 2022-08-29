namespace ShareMyCalendar.API.Requests;

public record ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
