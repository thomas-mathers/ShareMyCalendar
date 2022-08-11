using ShareMyCalendar.API.Mappers;
using ThomasMathers.Infrastructure.IAM.Data;
using Xunit;

namespace ShareMyCalendar.API.Tests
{
    public class RegisterSuccessResponseMapperTests
    {
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3")]
        public void Map_AnyInput_ReturnsCorrectOutput(string id)
        {
            // Arrange
            var user = new User
            {
                Id = Guid.Parse(id)
            };
            var source = new ThomasMathers.Infrastructure.IAM.Responses.RegisterSuccessResponse(user);

            // Act
            var actual = RegisterSuccessResponseMapper.Map(source);

            // Assert
            Assert.Equal(user.Id, actual.Id);
        }
    }
}