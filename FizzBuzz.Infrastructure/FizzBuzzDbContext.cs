using FizzBuzz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Infrastructure;

public class FizzBuzzDbContext(DbContextOptions<FizzBuzzDbContext> options) : DbContext(options)
{
    public DbSet<MetricEntity> Metrics { get; set; }
}
