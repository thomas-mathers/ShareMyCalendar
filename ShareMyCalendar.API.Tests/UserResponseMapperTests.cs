using AutoFixture;

using ShareMyCalendar.API.Mappers;

using ThomasMathers.Infrastructure.IAM.Data.EF;

using Xunit;

namespace ShareMyCalendar.API.Tests;

public class UserResponseMapperTests
{
    private readonly Fixture _fixture;

    public UserResponseMapperTests() => _fixture = new Fixture();

    [Fact]
    public void Map_AnyInput_ReturnsCorrectOutput()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        var actual = UserResponseMapper.Map(user);

        // Assert
        Assert.Equal(actual.Id, actual.Id);
        Assert.Equal(actual.UserName, actual.UserName);
        Assert.Equal(actual.Email, actual.Email);
        Assert.Equal(actual.EmailConfirmed, actual.EmailConfirmed);
        Assert.Equal(actual.PhoneNumber, actual.PhoneNumber);
        Assert.Equal(actual.PhoneNumberConfirmed, actual.PhoneNumberConfirmed);
    }
}