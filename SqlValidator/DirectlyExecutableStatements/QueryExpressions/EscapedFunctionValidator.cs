
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class EscapedFunctionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (input.Length < 3 || input is not ['{', 'f', 'n', .. ReadOnlySpan<char> tempRemainder])
        {
            remainder = input;
            return false;
        }

        remainder = tempRemainder;
        return true;
    }
}
