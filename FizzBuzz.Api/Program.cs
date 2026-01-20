using FizzBuzz.Domain.Repositories;
using FizzBuzz.Domain.Services;
using FizzBuzz.Infrastructure;
using FizzBuzz.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IFizzBuzzService, FizzBuzzService>();
builder.Services.AddScoped<IMetricService, MetricService>();

builder.Services.AddDbContext<FizzBuzzDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FizzBuzzDb")));

builder.Services.AddScoped<IMetricRepository, MetricRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
