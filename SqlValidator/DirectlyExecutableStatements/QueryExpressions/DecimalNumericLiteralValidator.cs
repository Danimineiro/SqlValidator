
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class DecimalNumericLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input.TrimStart();
        while (DigitValidator.Validate(remainder[0]))
        {
            remainder = remainder[1..];
        }

        if (remainder.Length < 2 || remainder[0] != '.')
        {
            remainder = input;
            return false;
        }

        remainder = remainder[1..];

        return UnsignedIntegerLiteralValidator.Validate(remainder, out remainder);
    }
}
