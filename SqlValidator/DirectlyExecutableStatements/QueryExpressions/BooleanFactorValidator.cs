
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BooleanFactorValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        //Remove "NOT" token if present
        input.HasNextToken("NOT", out remainder);

        return BooleanPrimaryValidator.Validate(remainder, out remainder);
    }
}
