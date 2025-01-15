namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;
public static class QueryTermValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return QueryPrimaryValidator.Validate(input, out remainder);
    }
}
