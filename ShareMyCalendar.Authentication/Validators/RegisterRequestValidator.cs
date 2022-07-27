using FluentValidation;
using ShareMyCalendar.API.Requests;
using ThomasMathers.Infrastructure.IAM.Extensions;
using ThomasMathers.Infrastructure.IAM.Settings;

namespace ShareMyCalendar.API.Validators
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
