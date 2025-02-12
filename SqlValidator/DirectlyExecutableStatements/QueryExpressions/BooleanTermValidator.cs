
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BooleanTermValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!BooleanFactorValidator.Validate(input, out remainder)) return false;

        while (remainder.HasNextToken("AND", out remainder))
        {
            if (!BooleanFactorValidator.Validate(remainder, out remainder)) return false;
        }

        return true;
    }
}
