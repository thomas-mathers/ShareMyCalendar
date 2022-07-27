using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.API.Requests
{
    public record ChangePasswordRequest
    {
        public string CurrentPassword { get; init; }
        public string Token { get; init; }
        public string NewPassword { get; init; }
    }
}
