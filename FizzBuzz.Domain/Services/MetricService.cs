using FizzBuzz.Domain.Dtos;
using FizzBuzz.Domain.Repositories;

namespace FizzBuzz.Domain.Services;

public class MetricService(IMetricRepository metricRepository) : IMetricService
{
    public async Task Increment(string query) => await metricRepository.Increment(query);

    public async Task<MetricDto?> GetMostHit()
    {
        var mostHit = await metricRepository.GetMostHit();
        if (mostHit == null)
        {
            return null;
        }

        return new MetricDto
        {
            Query = mostHit.Query,
            Count = mostHit.Count
        };
    }
}
