
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnsignedIntegerLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (input.Length < 1 || !DigitValidator.Validate(remainder[0])) return false;

        while (DigitValidator.Validate(remainder[0]))
        {
            remainder = remainder[1..];
        }

        return true;
    }
}
