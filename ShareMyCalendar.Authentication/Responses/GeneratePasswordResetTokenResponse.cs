using OneOf;

namespace ShareMyCalendar.Authentication.Responses
{
    [GenerateOneOf]
    public partial class GeneratePasswordResetTokenResponse : OneOfBase<NotFoundResponse, string>
    {
    }
}
