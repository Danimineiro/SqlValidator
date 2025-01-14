
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

internal class QueryPrimaryValidator
{
    internal static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return QueryValidator.Validate(input, out remainder);
    }
}
