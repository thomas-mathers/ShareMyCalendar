namespace ShareMyCalendar.API.Models;

public static class Constants
{
    public static class Errors
    {
        public static string IdentityError => "IdentityError";

        public static string UserLockedOut => "UserLockedOut";

        public static string LoginRequiresTwoFactor => "LoginRequiresTwoFactor";

        public static string LoginIsNotAllowed => "LoginIsNotAllowed";

        public static string IncorrectPassword => "LoginFailed";
    }
}
