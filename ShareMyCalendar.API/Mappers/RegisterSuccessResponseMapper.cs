using ShareMyCalendar.API.Responses;

namespace ShareMyCalendar.API.Mappers
{
    public static class RegisterSuccessResponseMapper
    {
        public static RegisterSuccessResponse Map(ThomasMathers.Infrastructure.IAM.Responses.RegisterSuccessResponse source)
        {
            return new RegisterSuccessResponse
            {
                Id = source.User.Id
            };
        }
    }
}
