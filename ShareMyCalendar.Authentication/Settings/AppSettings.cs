namespace ShareMyCalendar.Authentication.Settings
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public JwtTokenSettings Jwt { get; set; }

        public static AppSettings FromConfigurationSection(IConfigurationSection section)
        {
            var settings = new AppSettings();
            section.Bind(settings);
            return settings;
        }
    }
}
