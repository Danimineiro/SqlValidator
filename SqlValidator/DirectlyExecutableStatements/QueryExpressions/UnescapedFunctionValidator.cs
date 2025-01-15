
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnescapedFunctionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        return true;
    }
}
