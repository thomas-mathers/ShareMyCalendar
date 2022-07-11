using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record ChangePasswordRequest
    {
        [Required]
        public string UserName { get; init; } = string.Empty;
        public string CurrentPassword { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        [Required]
        public string NewPassword { get; init; } = string.Empty;
    }
}
