using OneOf;

namespace ShareMyCalendar.Authentication.Responses
{
    [GenerateOneOf]
    public partial class RegisterResponse : OneOfBase<IdentityErrorResponse, RegisterSuccessResponse>
    {
    }
}
