
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnsignedNumericLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (ApproximateNumericLiteralValidator.Validate(input, out remainder)) return true;

        if (UnsignedIntegerLiteralValidator.Validate(input, out remainder)) return true;

        return DecimalNumericLiteralValidator.Validate(input, out remainder);
    }
}
