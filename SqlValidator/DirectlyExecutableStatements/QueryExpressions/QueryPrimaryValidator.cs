
using SqlValidator.Identifiers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class QueryPrimaryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (QueryValidator.Validate(input, out remainder)) return true;
        if (CheckIfValues(input, out remainder)) return true;
        if (CheckIfTable(input, out remainder)) return true;
        return CheckIfExpression(input, out remainder);
    }

    private static bool CheckIfValues(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!input.HasNextToken("VALUES", out remainder)) return false;
        if (remainder[0] != '(') return false;
        remainder = remainder[1..];

        if (!ExpressionListValidator.Validate(remainder, out remainder)) return false;
        if (remainder[0] != ')') return false;
        remainder = remainder[1..];

        while (remainder[0] == ',')
        {
            remainder = remainder[1..];

            if (remainder[0] != '(') return false;
            remainder = remainder[1..];

            if (!ExpressionListValidator.Validate(remainder, out remainder)) return false;
            if (remainder[0] != ')') return false;
            remainder = remainder[1..];
        }

        return true;
    }

    private static bool CheckIfTable(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!input.HasNextToken("TABLE", out remainder)) return false;
        return IdentifierValidator.Validate(remainder, out remainder);
    }

    private static bool CheckIfExpression(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        throw new NotImplementedException();
    }
}
