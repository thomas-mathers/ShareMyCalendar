using FluentValidation;
using ShareMyCalendar.Authentication.Requests;
using ThomasMathers.Common.IAM.Extensions;
using ThomasMathers.Common.IAM.Settings;

namespace ShareMyCalendar.Authentication.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(UserSettings userSettings, PasswordSettings passwordSettings)
        {
            RuleFor(x => x.UserName).Username(userSettings);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Password(passwordSettings);
        }
    }
}
