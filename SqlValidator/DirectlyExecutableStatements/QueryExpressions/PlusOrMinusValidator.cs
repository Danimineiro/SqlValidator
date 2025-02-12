namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class PlusOrMinusValidator
{
    private static readonly char[] ValidChars = ['+', '-'];

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (input.Length < 1 || !ValidChars.Contains(input[0]))
        {
            remainder = input;
            return false;
        }

        remainder = input[1..];
        return true;
    }
}
