using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record LoginRequest
    {
        [Required]
        public string UserName { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
    }
}
