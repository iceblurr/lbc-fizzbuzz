using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FizzBuzzController : ControllerBase
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
        ValidateParameters(limit, divisor1, divisor2);

        var result = new string[limit];
        for (var i = 0; i < limit; i++)
        {
            var sentence = GetResult(i + 1, divisor1, replacement1, divisor2, replacement2);
            result[i] = sentence;
        }

        return result;
    }

    private static void ValidateParameters(int limit, int divisor1, int divisor2)
    {
        if (limit < MinLimit)
        {
            throw new InvalidOperationException($"Limit must be at least {MinLimit}.");
        }

        if (limit > MaxLimit)
        {
            throw new InvalidOperationException($"Impossible to proceed more than {MaxLimit} elements");
        }

        if (divisor1 == 0 || divisor2 == 0)
        {
            throw new InvalidOperationException("Impossible to divide by 0");
        }
    }

    private static bool IsMultipleOf(int number, int divisor) => number % divisor == 0;

    private static string GetResult(
        int number,
        int divisor1, string sentence1,
        int divisor2, string sentence2)
    {
        if (IsMultipleOf(number, divisor1))
        {
            if (IsMultipleOf(number, divisor2))
            {
                return sentence1 + sentence2;
            }

            return sentence1;
        }

        if (IsMultipleOf(number, divisor2))
        {
            return sentence2;
        }

        return number.ToString();
    }
}
