using OneOf;

namespace ShareMyCalendar.Authentication.Responses
{
    [GenerateOneOf]
    public partial class LoginResponse : OneOfBase<NotFoundResponse, UserLockedOutResponse, LoginRequiresTwoFactorResponse, LoginIsNotAllowedResponse, LoginFailureResponse, LoginSuccessResponse>
    {
    }
}
