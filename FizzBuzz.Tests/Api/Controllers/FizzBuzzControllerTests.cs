using FizzBuzz.Api.Controllers;
using FizzBuzz.Domain.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FizzBuzz.Tests.Api.Controllers;

public class FizzBuzzControllerTests
{
    private readonly FizzBuzzController _controller;
    private readonly IFizzBuzzService _fizzBuzzServiceMock;
    private readonly IMetricService _metricServiceMock;

    public FizzBuzzControllerTests()
    {
        _fizzBuzzServiceMock = Substitute.For<IFizzBuzzService>();
        _metricServiceMock = Substitute.For<IMetricService>();
        _controller = new FizzBuzzController(_fizzBuzzServiceMock, _metricServiceMock);

        // Setup HttpContext for Request.QueryString
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetFizzBuzz_ValidParameters_ReturnsCorrectNumberOfResults()
    {
        // Arrange
        const int limit = 3;
        const int int1 = 3, int2 = 5;
        const string str1 = "fizz", str2 = "buzz";
        var queryString = $"?int1={int1}&int2={int2}&limit={limit}&str1={str1}&str2={str2}";
        _controller.HttpContext.Request.QueryString = new QueryString(queryString);

        _fizzBuzzServiceMock
            .GetFizzBuzzResult(Arg.Any<int>(), int1, str1, int2, str2)
            .Returns("result");

        // Act
        var result = (await _controller.GetFizzBuzz(int1, int2, limit, str1, str2)).ToList();

        // Assert
        result.Should().HaveCount(limit);
        _fizzBuzzServiceMock
            .Received(limit)
            .GetFizzBuzzResult(Arg.Any<int>(), int1, str1, int2, str2);
    }

    [Fact]
    public async Task GetFizzBuzz_ValidParameters_IncrementsMetric()
    {
        // Arrange
        const int limit = 3;
        const int int1 = 3, int2 = 5;
        const string str1 = "fizz", str2 = "buzz";
        var queryString = $"?int1={int1}&int2={int2}&limit={limit}&str1={str1}&str2={str2}";
        _controller.HttpContext.Request.QueryString = new QueryString(queryString);

        // Act
        await _controller.GetFizzBuzz(int1, int2, limit, str1, str2);

        // Assert
        await _metricServiceMock
            .Received(1)
            .Increment(Arg.Is<string>(s => s.Contains(queryString)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetFizzBuzz_LimitTooLow_ThrowsInvalidOperationException(int limit)
    {
        // Arrange
        _controller.HttpContext.Request.QueryString = new QueryString($"?limit={limit}");

        // Act
        var act = async () => await _controller.GetFizzBuzz(3, 5, limit, "fizz", "buzz");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Limit must be at least 1.");
    }

    [Fact]
    public async Task GetFizzBuzz_LimitTooHigh_ThrowsInvalidOperationException()
    {
        // Arrange
        const int limit = 1_000_001;
        _controller.HttpContext.Request.QueryString = new QueryString($"?limit={limit}");

        // Act
        var act = async () => await _controller.GetFizzBuzz(3, 5, limit, "fizz", "buzz");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Impossible to proceed more than 1000000 elements");
    }

    [Fact]
    public async Task GetFizzBuzz_CallsServiceWithCorrectSequenceOfNumbers()
    {
        // Arrange
        const int limit = 2;
        const int int1 = 3, int2 = 5;
        const string str1 = "fizz", str2 = "buzz";
        var queryString = $"?int1={int1}&int2={int2}&limit={limit}&str1={str1}&str2={str2}";
        _controller.HttpContext.Request.QueryString = new QueryString(queryString);

        // Act
        (await _controller.GetFizzBuzz(int1, int2, limit, str1, str2)).ToList();

        // Assert
        Received.InOrder(() =>
        {
            _fizzBuzzServiceMock.GetFizzBuzzResult(1, int1, str1, int2, str2);
            _fizzBuzzServiceMock.GetFizzBuzzResult(2, int1, str1, int2, str2);
        });
    }
}
