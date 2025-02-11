
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class RBraceValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (input.Length < 1 || input[0] != '}') return false;

        remainder = remainder[1..];
        return true;
    }
}
