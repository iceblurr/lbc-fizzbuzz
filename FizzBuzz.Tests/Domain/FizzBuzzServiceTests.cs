using FizzBuzz.Api.Domain;

namespace FizzBuzz.Tests.Domain;

public class FizzBuzzServiceTests
{
    private readonly FizzBuzzService _service = new();

    [Theory]
    [InlineData(3, 3, "fizz", 5, "buzz", "fizz")]
    [InlineData(5, 3, "fizz", 5, "buzz", "buzz")]
    [InlineData(15, 3, "fizz", 5, "buzz", "fizzbuzz")]
    [InlineData(7, 3, "fizz", 5, "buzz", "7")]
    public void GetFizzBuzzResult_ReturnsExpectedString(
        int number, int int1, string str1, int int2, string str2, string expected)
    {
        // Act
        var result = _service.GetFizzBuzzResult(number, int1, str1, int2, str2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFizzBuzzResult_WithCustomStrings_ReturnsConcatenatedCustomStrings()
    {
        // Arrange
        const int number = 6;
        const int int1 = 2, int2 = 3;
        const string str1 = "Apple", str2 = "Pie";

        // Act
        var result = _service.GetFizzBuzzResult(number, int1, str1, int2, str2);

        // Assert
        Assert.Equal("ApplePie", result);
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(3, 0)]
    [InlineData(0, 0)]
    public void GetFizzBuzzResult_DivisorIsZero_ThrowsInvalidOperationException(int int1, int int2)
    {
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            _service.GetFizzBuzzResult(10, int1, "fizz", int2, "buzz"));

        Assert.Equal("Impossible to divide by 0", exception.Message);
    }
}
