using OneOf;

namespace ShareMyCalendar.Authentication.Responses
{
    [GenerateOneOf]
    public partial class ResetPasswordResponse : OneOfBase<NotFoundResponse, IdentityErrorResponse, ResetPasswordSuccessResponse>
    {
    }
}
