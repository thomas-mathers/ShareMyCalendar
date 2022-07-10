using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record LoginRequest
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
