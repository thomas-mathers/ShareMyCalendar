using FluentValidation;
using ShareMyCalendar.API.Requests;
using ThomasMathers.Infrastructure.IAM.Extensions;
using ThomasMathers.Infrastructure.IAM.Settings;

namespace ShareMyCalendar.API.Validators
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
