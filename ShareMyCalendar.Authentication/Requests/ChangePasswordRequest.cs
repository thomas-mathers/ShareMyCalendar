using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record ChangePasswordRequest
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string CurrentPassword { get; init; }
        [Required]
        public string NewPassword { get; init; }
    }
}
