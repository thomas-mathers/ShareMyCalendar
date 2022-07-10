using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record ResetPasswordRequest
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string Token { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
