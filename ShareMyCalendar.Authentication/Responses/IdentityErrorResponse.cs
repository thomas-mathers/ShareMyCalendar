using Microsoft.AspNetCore.Identity;

namespace ShareMyCalendar.Authentication.Responses
{
    public record IdentityErrorResponse(IEnumerable<IdentityError> Errors);
}
