using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record GeneratePasswordResetTokenRequest
    {
        [Required]
        public string UserName { get; init; } = string.Empty;
    }
}
