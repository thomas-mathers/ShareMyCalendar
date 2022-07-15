namespace ShareMyCalendar.Authentication.Requests
{
    public record RegisterRequest
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
