using AutoFixture;

using ShareMyCalendar.API.Mappers;

using ThomasMathers.Infrastructure.IAM.Responses;

using Xunit;

namespace ShareMyCalendar.API.Tests;

public class LoginSuccessResponseMapperTests
{
    private readonly Fixture _fixture;

    public LoginSuccessResponseMapperTests() => _fixture = new Fixture();

    [Fact]
    public void Map_AnyInput_ReturnsCorrectOutput()
    {
        // Arrange
        var expected = _fixture.Create<LoginSuccessResponse>();

        // Act
        var actual = LoginSuccessResponseMapper.Map(expected);

        // Assert
        Assert.Equal(expected.User.Id, actual.Id);
        Assert.Equal(expected.User.UserName, actual.UserName);
        Assert.Equal(expected.AccessToken, actual.AccessToken);
    }
}