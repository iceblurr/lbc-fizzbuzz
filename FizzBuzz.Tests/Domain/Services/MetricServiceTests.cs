using FizzBuzz.Domain.Entities;
using FizzBuzz.Domain.Repositories;
using FizzBuzz.Domain.Services;
using FluentAssertions;
using NSubstitute;

namespace FizzBuzz.Tests.Domain.Services;

public class MetricServiceTests
{
    private readonly MetricService _service;
    private readonly IMetricRepository _metricRepositoryMock;

    public MetricServiceTests()
    {
        _metricRepositoryMock = Substitute.For<IMetricRepository>();
        _service = new MetricService(_metricRepositoryMock);
    }

    [Fact]
    public async Task Increment_CallsRepositoryWithCorrectQuery()
    {
        // Arrange
        const string query = "int1=3&int2=5&limit=10&str1=fizz&str2=buzz";

        // Act
        await _service.Increment(query);

        // Assert
        await _metricRepositoryMock.Received(1).Increment(query);
    }

    [Fact]
    public async Task GetMostHit_WhenRepositoryReturnsNull_ReturnsNull()
    {
        // Arrange
        _metricRepositoryMock.GetMostHit().Returns((MetricEntity?)null);

        // Act
        var result = await _service.GetMostHit();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMostHit_WhenRepositoryReturnsEntity_ReturnsMappedDto()
    {
        // Arrange
        var entity = new MetricEntity
        {
            Query = "int1=3&int2=5&limit=10&str1=fizz&str2=buzz",
            Count = 42
        };
        _metricRepositoryMock.GetMostHit().Returns(entity);

        // Act
        var result = await _service.GetMostHit();

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().Be(entity.Query);
        result.Count.Should().Be(entity.Count);
    }
}
