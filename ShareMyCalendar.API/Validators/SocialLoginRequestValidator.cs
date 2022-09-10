using FluentValidation;
using ShareMyCalendar.API.Requests;

namespace ShareMyCalendar.API.Validators;

public class SocialLoginRequestValidator : AbstractValidator<SocialLoginRequest>
{
    public SocialLoginRequestValidator()
    {
        _ = RuleFor(x => x.Provider).NotEmpty();
        _ = RuleFor(x => x.UserId).NotEmpty();
        _ = RuleFor(x => x.AccessToken).NotEmpty();
    }
}
