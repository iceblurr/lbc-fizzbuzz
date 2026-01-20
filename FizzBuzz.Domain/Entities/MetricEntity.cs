using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FizzBuzz.Domain.Entities;

[Table("fizzbuzz_metric")]
public class MetricEntity
{
    [Key] [Column("query")] public string Query { get; set; } = string.Empty;

    [Column("count")] public int Count { get; set; }
}
