
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ConditionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return BooleanValueExpressionValidator.Validate(input, out remainder);
    }
}
