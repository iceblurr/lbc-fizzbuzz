namespace FizzBuzz.Domain.Services;

public interface IFizzBuzzService
{
    string GetFizzBuzzResult(
        int number,
        int divisor1, string sentence1,
        int divisor2, string sentence2);
}
