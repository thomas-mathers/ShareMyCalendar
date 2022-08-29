using FluentValidation;

using ShareMyCalendar.API.Requests;

namespace ShareMyCalendar.API.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        _ = RuleFor(x => x.UserName).NotEmpty();
        _ = RuleFor(x => x.Password).NotEmpty();
    }
}
