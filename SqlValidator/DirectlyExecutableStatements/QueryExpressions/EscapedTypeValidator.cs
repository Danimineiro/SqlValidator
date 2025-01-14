
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class EscapedTypeValidator
{
    private static readonly char[] singleValidChar = ['d', 't', 'b'];

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input.TrimStart();
        if (remainder.Length < 2 || remainder[0] != '{')
        {
            remainder = input;
            return false;
        }

        remainder = remainder[1..];

        if (singleValidChar.Contains(remainder[0]))
        {
            if (remainder[0] != 't')
            {
                remainder = remainder[1..];
                return true;
            }

            if (remainder.Length >= 2 && remainder[1] == 's')
            {
                remainder = remainder[2..];
                return true;
            }
        }

        remainder = input;
        return false;
    }
}
