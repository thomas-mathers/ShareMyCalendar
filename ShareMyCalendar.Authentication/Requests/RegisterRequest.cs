using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.Authentication.Requests
{
    public record RegisterRequest
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
