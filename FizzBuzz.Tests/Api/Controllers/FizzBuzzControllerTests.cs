using FizzBuzz.Api.Controllers;
using FizzBuzz.Domain.Services;
using NSubstitute;

namespace FizzBuzz.Tests.Api.Controllers;

public class FizzBuzzControllerTests
{
    private readonly FizzBuzzController _controller;
    private readonly IFizzBuzzService _fizzBuzzServiceMock;

    public FizzBuzzControllerTests()
    {
        _fizzBuzzServiceMock = Substitute.For<IFizzBuzzService>();
        _controller = new FizzBuzzController(_fizzBuzzServiceMock);
    }

    [Fact]
    public void GetFizzBuzz_ValidParameters_ReturnsCorrectNumberOfResults()
    {
        // Arrange
        const int limit = 3;
        const int int1 = 3, int2 = 5;
        const string str1 = "fizz", str2 = "buzz";

        _fizzBuzzServiceMock
            .GetFizzBuzzResult(Arg.Any<int>(), int1, str1, int2, str2)
            .Returns("result");

        // Act
        var result = _controller.GetFizzBuzz(int1, int2, limit, str1, str2).ToList();

        // Assert
        Assert.Equal(limit, result.Count);
        _fizzBuzzServiceMock
            .Received(limit)
            .GetFizzBuzzResult(Arg.Any<int>(), int1, str1, int2, str2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GetFizzBuzz_LimitTooLow_ThrowsInvalidOperationException(int limit)
    {
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            _controller.GetFizzBuzz(3, 5, limit, "fizz", "buzz"));

        Assert.Contains("Limit must be at least 1", exception.Message);
    }

    [Fact]
    public void GetFizzBuzz_LimitTooHigh_ThrowsInvalidOperationException()
    {
        // Arrange
        const int limit = 1_000_001;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            _controller.GetFizzBuzz(3, 5, limit, "fizz", "buzz"));

        Assert.Contains("Impossible to proceed more than 1000000 elements", exception.Message);
    }

    [Fact]
    public void GetFizzBuzz_CallsServiceWithCorrectSequenceOfNumbers()
    {
        // Arrange
        const int limit = 2;
        const int int1 = 3, int2 = 5;
        const string str1 = "fizz", str2 = "buzz";

        // Act
        _controller.GetFizzBuzz(int1, int2, limit, str1, str2).ToList();

        // Assert
        Received.InOrder(() =>
        {
            _fizzBuzzServiceMock.GetFizzBuzzResult(1, int1, str1, int2, str2);
            _fizzBuzzServiceMock.GetFizzBuzzResult(2, int1, str1, int2, str2);
        });
    }
}
