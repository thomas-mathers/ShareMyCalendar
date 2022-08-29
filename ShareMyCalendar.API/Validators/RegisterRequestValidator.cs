using FluentValidation;

using ShareMyCalendar.API.Requests;

using ThomasMathers.Infrastructure.IAM.Extensions;
using ThomasMathers.Infrastructure.IAM.Settings;

namespace ShareMyCalendar.API.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(UserSettings userSettings, PasswordSettings passwordSettings)
    {
        _ = RuleFor(x => x.UserName).Username(userSettings);
        _ = RuleFor(x => x.Email).EmailAddress();
        _ = RuleFor(x => x.Password).Password(passwordSettings);
    }
}
