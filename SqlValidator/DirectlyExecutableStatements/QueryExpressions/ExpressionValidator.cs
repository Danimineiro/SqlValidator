
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return ConditionValidator.Validate(input, out remainder);
    }
}
