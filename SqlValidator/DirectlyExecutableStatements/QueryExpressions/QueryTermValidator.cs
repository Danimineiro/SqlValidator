namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;
public static class QueryTermValidator
{
    private static readonly string[] AllOrDistinct = ["ALL", "DISTINCT"];

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!QueryPrimaryValidator.Validate(input, out remainder)) return false;
        
        while (remainder.HasNextToken("INTERSECT", out remainder))
        {
            //Remove optionals
            remainder.HasAnyNextToken(AllOrDistinct, out remainder);

            if (!QueryPrimaryValidator.Validate(remainder, out remainder)) return false;
        }

        return true;
    }
}
