
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnsignedIntegerLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input.TrimStart();
        if (input.Length < 1)
        {
            remainder = input;
            return false;
        }

        while (DigitValidator.Validate(remainder[0]))
        {
            remainder = remainder[1..];
        }

        return true;
    }
}
