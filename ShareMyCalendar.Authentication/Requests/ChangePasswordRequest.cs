using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record ChangePasswordRequest
    {
        public string CurrentPassword { get; init; }
        public string Token { get; init; }
        public string NewPassword { get; init; }
    }
}
