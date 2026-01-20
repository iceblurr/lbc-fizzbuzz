using FizzBuzz.Domain.Dtos;

namespace FizzBuzz.Domain.Services;

public interface IMetricService
{
    Task Increment(string query);
    Task<MetricDto?> GetMostHit();
}
