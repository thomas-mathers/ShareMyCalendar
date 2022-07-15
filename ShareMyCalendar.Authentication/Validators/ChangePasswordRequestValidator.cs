using FluentValidation;
using ShareMyCalendar.Authentication.Requests;
using ThomasMathers.Common.IAM.Extensions;
using ThomasMathers.Common.IAM.Settings;

namespace ShareMyCalendar.Authentication.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(PasswordSettings passwordSettings)
        {
            RuleFor(x => x.NewPassword).Password(passwordSettings);
            RuleFor(x => x.CurrentPassword).NotEmpty().When(x => string.IsNullOrEmpty(x.Token));
            RuleFor(x => x.Token).NotEmpty().When(x => string.IsNullOrEmpty(x.CurrentPassword));
        }
    }
}
