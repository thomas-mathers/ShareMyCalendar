using ShareMyCalendar.API.Mappers;
using ThomasMathers.Infrastructure.IAM.Data;
using Xunit;

namespace ShareMyCalendar.API.Tests
{
    public class LoginSuccessResponseMapperTests
    {
        [Theory]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, null)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, "")]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", null)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", "")]
        [InlineData("7c54525c-05cf-48c9-bc8f-b778fa6744ed", "tmathers", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c")]
        public void Map_AnyInput_ReturnsCorrectOutput(string userId, string userName, string accessToken)
        {
            // Arrange
            var user = new User
            {
                Id = Guid.Parse(userId),
                UserName = userName
            };
            var source = new ThomasMathers.Infrastructure.IAM.Responses.LoginSuccessResponse(user, accessToken);

            // Act
            var actual = LoginSuccessResponseMapper.Map(source);

            // Assert
            Assert.Equal(user.Id, actual.Id);
            Assert.Equal(userName, actual.UserName);
            Assert.Equal(accessToken, actual.AccessToken);
        }
    }
}