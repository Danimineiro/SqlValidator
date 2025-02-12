
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class NumericValueExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!TermValidator.Validate(input, out remainder)) return false;

        while (PlusOrMinusValidator.Validate(remainder, out remainder))
        {
            if (TermValidator.Validate(remainder, out remainder)) continue;

            remainder = input;
            return false;
        }

        return true;
    }
}
