using System.ComponentModel.DataAnnotations;

namespace ShareMyCalendar.API.Requests
{
    public record LoginRequest
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
