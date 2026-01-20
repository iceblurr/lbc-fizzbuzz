using FizzBuzz.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FizzBuzzController(
    IFizzBuzzService fizzBuzzService,
    IMetricService metricService) : ControllerBase
{
    private const int MinLimit = 1;
    private const int MaxLimit = 1000000;

    /// <summary>
    ///     Returns a list of strings with numbers from 1 to limit, where:
    ///     - all multiples of int1 are replaced by str1,
    ///     - all multiples of int2 are replaced by str2,
    ///     - all multiples of int1 and int2 are replaced by str1str2.
    /// </summary>
    /// <param name="divisor1"></param>
    /// <param name="divisor2"></param>
    /// <param name="limit"></param>
    /// <param name="replacement1"></param>
    /// <param name="replacement2"></param>
    /// <returns>a list of strings</returns>
    [HttpGet(Name = "GetFizzBuzz")]
    public IEnumerable<string> GetFizzBuzz(
        [FromQuery(Name = "int1")] int divisor1,
        [FromQuery(Name = "int2")] int divisor2,
        int limit,
        [FromQuery(Name = "str1")] string replacement1,
        [FromQuery(Name = "str2")] string replacement2)
    {
        metricService.Increment(Request.QueryString.ToString());

        ValidateParameters(limit);

        var result = new string[limit];
        for (var i = 0; i < limit; i++)
        {
            var sentence = fizzBuzzService.GetFizzBuzzResult(i + 1, divisor1, replacement1, divisor2, replacement2);
            result[i] = sentence;
        }

        return result;
    }

    private static void ValidateParameters(int limit)
    {
        if (limit < MinLimit)
        {
            throw new InvalidOperationException($"Limit must be at least {MinLimit}.");
        }

        if (limit > MaxLimit)
        {
            throw new InvalidOperationException($"Impossible to proceed more than {MaxLimit} elements");
        }
    }
}
