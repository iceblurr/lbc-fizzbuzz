using FizzBuzz.Domain.Entities;
using FizzBuzz.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Infrastructure.Repositories;

public class MetricRepository(FizzBuzzDbContext context) : IMetricRepository
{
    public async Task Increment(string query)
    {
        var metric = await context.Metrics.FindAsync(query);
        if (metric == null)
        {
            metric = new MetricEntity { Query = query, Count = 1 };
            context.Metrics.Add(metric);
        }
        else
        {
            metric.Count++;
        }

        await context.SaveChangesAsync();
    }

    public async Task<MetricEntity?> GetMostHit()
    {
        var metric = await context.Metrics
            .OrderByDescending(m => m.Count)
            .FirstOrDefaultAsync();

        return metric;
    }
}
