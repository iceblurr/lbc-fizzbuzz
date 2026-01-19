namespace FizzBuzz.Api.Domain;

public class FizzBuzzService : IFizzBuzzService
{
    public string GetFizzBuzzResult(
        int number,
        int divisor1, string sentence1,
        int divisor2, string sentence2)
    {
        if (divisor1 == 0 || divisor2 == 0)
        {
            throw new InvalidOperationException("Impossible to divide by 0");
        }

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

    private static bool IsMultipleOf(int number, int divisor) => number % divisor == 0;
}
