namespace ShareMyCalendar.Authentication.Options
{
    public class AccessTokenGeneratorOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifespanInDays { get; set; }
    }
}
