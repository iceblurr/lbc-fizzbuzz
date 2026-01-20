using FizzBuzz.Api.Controllers;
using FizzBuzz.Domain.Dtos;
using FizzBuzz.Domain.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FizzBuzz.Tests.Api.Controllers;

public class MetricControllerTests
{
    private readonly MetricController _controller;
    private readonly IMetricService _metricServiceMock;

    public MetricControllerTests()
    {
        _metricServiceMock = Substitute.For<IMetricService>();
        _controller = new MetricController(_metricServiceMock);
    }

    [Fact]
    public async Task GetMostHit_WhenNoMetricsExist_ReturnsNoContent()
    {
        // Arrange
        _metricServiceMock.GetMostHit().Returns((MetricDto?)null);

        // Act
        var result = await _controller.GetMostHit();

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetMostHit_WhenMetricsExist_ReturnsOkWithResult()
    {
        // Arrange
        var metricDto = new MetricDto
        {
            Query = "?int1=3&int2=5&limit=100&str1=fizz&str2=buzz",
            Count = 10
        };
        _metricServiceMock.GetMostHit().Returns(metricDto);

        // Act
        var result = await _controller.GetMostHit();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(metricDto);
    }
}
