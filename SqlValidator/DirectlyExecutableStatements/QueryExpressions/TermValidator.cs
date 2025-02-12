
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class TermValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!ValueExpressionPrimaryValidator.Validate(input, out remainder)) return false;

        while (StarOrSlashValidator.Validate(remainder, out remainder))
        {
            if (ValueExpressionPrimaryValidator.Validate(remainder, out remainder)) continue;

            remainder = input;
            return false;
        }

        return true;
    }
}
