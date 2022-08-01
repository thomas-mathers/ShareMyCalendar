using ThomasMathers.Infrastructure.IAM.Data;

namespace ShareMyCalendar.API
{
    public static class Roles
    {
        public static readonly Role Admin = new()
        {
            Id = Guid.Parse("7252f51b-53a8-4714-8f0c-c7ee5ecbf8e9"),
            Name = "admin",
            NormalizedName = "ADMIN"
        };

        public static readonly Role User = new()
        {
            Id = Guid.Parse("5c74f9e8-fd47-4f31-831e-e55d266e6249"),
            Name = "user",
            NormalizedName = "USER"
        };
    }
}
