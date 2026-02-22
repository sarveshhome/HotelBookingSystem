using Common.Domain;
using FluentAssertions;
using Xunit;

namespace Common.Tests;

public class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessResult()
    {
        var result = Result<string>.Success("test");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("test");
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_CreatesFailureResult()
    {
        var result = Result<string>.Failure("error message");

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be("error message");
    }

    [Fact]
    public void Success_WithComplexType_ReturnsValue()
    {
        var data = new { Id = 1, Name = "Test" };
        var result = Result<object>.Success(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(data);
    }
}
