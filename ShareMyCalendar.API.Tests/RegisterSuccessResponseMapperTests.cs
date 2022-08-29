using AutoFixture;

using ShareMyCalendar.API.Mappers;

using ThomasMathers.Infrastructure.IAM.Responses;

using Xunit;

namespace ShareMyCalendar.API.Tests;

public class RegisterSuccessResponseMapperTests
{
    private readonly Fixture _fixture;

    public RegisterSuccessResponseMapperTests() => _fixture = new Fixture();

    [Fact]
    public void Map_AnyInput_ReturnsCorrectOutput()
    {
        // Arrange
        var source = _fixture.Create<RegisterSuccessResponse>();

        // Act
        var actual = RegisterSuccessResponseMapper.Map(source);

        // Assert
        Assert.Equal(source.User.Id, actual.Id);
    }
}