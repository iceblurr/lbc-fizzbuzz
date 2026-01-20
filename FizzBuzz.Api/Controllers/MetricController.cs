using FizzBuzz.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MetricController(IMetricService metricService) : ControllerBase
{
    [HttpGet("most-hit", Name = "GetMostHit")]
    public async Task<IActionResult> GetMostHit()
    {
        var result = await metricService.GetMostHit();
        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }
}
