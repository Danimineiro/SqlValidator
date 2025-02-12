
using SqlValidator.Identifiers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class WithListElementValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!IdentifierValidator.Validate(input, out remainder)) return false;
        if (!CheckOptionalColumnList(remainder, out remainder))
        {
            remainder = input;
            return false;
        }

        if (!remainder.HasNextToken("AS", out remainder))
        {
            remainder = input;
            return false;
        }

        if (remainder.TrimStart()[0] != '(')
        {
            remainder = input;
            return false;
        }

        if (!QueryExpressionValidator.Validate(remainder, out remainder))
        {
            remainder = input;
            return false;
        }

        if (remainder.TrimStart()[0] != ')')
        {
            remainder = input;
            return false;
        }

        return true;
    }

    private static bool CheckOptionalColumnList(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (input[0] != '(') return true;
        return ColumnListValidator.Validate(input, out remainder);
    }
}
