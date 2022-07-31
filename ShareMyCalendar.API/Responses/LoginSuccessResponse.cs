namespace ShareMyCalendar.API.Responses
{
    public class LoginSuccessResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
    }
}
