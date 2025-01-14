
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class SubQueryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (input.Length < 2 || input[0] != '(') return false;

        remainder = remainder[1..];
        if (!QueryExpressionValidator.Validate(remainder, out remainder) || !CallStatementValidator.Validate(remainder, out remainder))
        {
            remainder = input;
            return false;
        };
        
        if (remainder.Length >= 1 && remainder[0] == ')')
        {
            remainder = remainder[1..];
            return true;
        }

        remainder = input;
        return false;
    }
}
