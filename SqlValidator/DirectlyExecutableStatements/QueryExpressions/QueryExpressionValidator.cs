namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class QueryExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!CheckOptionalWith(input, out remainder)) return false;

        return QueryExpressionBodyValidator.Validate(remainder, out remainder);
    }

    private static bool CheckOptionalWith(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!input.HasNextToken("WITH", out remainder)) return true;

        if (!WithListElementValidator.Validate(remainder, out remainder)) return false;

        while (remainder[0] == ',')
        {
            if (!WithListElementValidator.Validate(remainder, out remainder)) return false;
        }

        return true;
    }
}
