namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;
public static class QueryExpressionBodyValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return QueryTermValidator.Validate(input, out remainder);
    }
}
