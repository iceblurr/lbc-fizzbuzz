using FizzBuzz.Domain.Entities;
using FizzBuzz.Infrastructure;
using FizzBuzz.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Tests.Infrastructure.Repositories;

public class MetricRepositoryTests
{
    private readonly FizzBuzzDbContext _context;
    private readonly MetricRepository _repository;

    public MetricRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FizzBuzzDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FizzBuzzDbContext(options);
        _repository = new MetricRepository(_context);
    }

    [Fact]
    public async Task Increment_WhenMetricDoesNotExist_CreatesNewMetric()
    {
        // Arrange
        const string query = "int1=3&int2=5&limit=10&str1=fizz&str2=buzz";

        // Act
        await _repository.Increment(query);

        // Assert
        var metric = await _context.Metrics.FindAsync(query);
        metric.Should().NotBeNull();
        metric!.Query.Should().Be(query);
        metric.Count.Should().Be(1);
    }

    [Fact]
    public async Task Increment_WhenMetricExists_IncrementsCount()
    {
        // Arrange
        const string query = "int1=3&int2=5&limit=10&str1=fizz&str2=buzz";
        _context.Metrics.Add(new MetricEntity { Query = query, Count = 5 });
        await _context.SaveChangesAsync();

        // Act
        await _repository.Increment(query);

        // Assert
        var metric = await _context.Metrics.FindAsync(query);
        metric.Should().NotBeNull();
        metric!.Count.Should().Be(6);
    }

    [Fact]
    public async Task GetMostHit_WhenNoMetricsExist_ReturnsNull()
    {
        // Act
        var result = await _repository.GetMostHit();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetMostHit_WhenMetricsExist_ReturnsMostHitMetric()
    {
        // Arrange
        _context.Metrics.AddRange(
            new MetricEntity { Query = "query1", Count = 5 },
            new MetricEntity { Query = "query2", Count = 10 },
            new MetricEntity { Query = "query3", Count = 2 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMostHit();

        // Assert
        result.Should().NotBeNull();
        result!.Query.Should().Be("query2");
        result.Count.Should().Be(10);
    }
}
