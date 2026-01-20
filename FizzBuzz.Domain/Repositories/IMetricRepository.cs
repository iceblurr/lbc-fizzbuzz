using FizzBuzz.Domain.Entities;

namespace FizzBuzz.Domain.Repositories;

public interface IMetricRepository
{
    Task Increment(string query);
    Task<MetricEntity?> GetMostHit();
}
