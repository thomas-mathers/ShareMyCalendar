using ShareMyCalendar.API.Mappers;
using ThomasMathers.Infrastructure.IAM.Data;
using Xunit;

namespace ShareMyCalendar.API.Tests
{
    public class UserResponseMapperTests
    {
        [Theory]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, null, false, null, false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, null, false, null, true)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, null, true, null, false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", null, null, true, null, true)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", "", false, "", false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", "", false, "", true)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", "", true, "", false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "", "", true, "", true)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "tmathers", "tmathers@gmail.com", false, "123-123-1234", false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "tmathers", "tmathers@gmail.com", false, "123-123-1234", true)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "tmathers", "tmathers@gmail.com", true, "123-123-1234", false)]
        [InlineData("42973335-774b-4136-8c8c-766000e5e4e3", "tmathers", "tmathers@gmail.com", true, "123-123-1234", true)]
        public void Map_AnyInput_ReturnsCorrectOutput(string userId, string userName, string email, bool emailConfirmed, string phoneNumber, bool phoneNumberConfirmed)
        {
            // Arrange
            var user = new User
            {
                Id = Guid.Parse(userId),
                UserName = userName,
                Email = email,
                EmailConfirmed = emailConfirmed,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = phoneNumberConfirmed
            };
            // Act
            var actual = UserResponseMapper.Map(user);

            // Assert
            Assert.Equal(user.Id, actual.Id);
            Assert.Equal(userName, actual.UserName);
            Assert.Equal(email, actual.Email);
            Assert.Equal(emailConfirmed, actual.EmailConfirmed);
            Assert.Equal(phoneNumber, actual.PhoneNumber);
            Assert.Equal(phoneNumberConfirmed, actual.PhoneNumberConfirmed);
        }
    }
}