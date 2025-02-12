
using SqlValidator.Identifiers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ColumnListValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (input[0] != '(') return false;

        if (!IdentifierValidator.Validate(remainder, out remainder))
        {
            remainder = input;
            return false;
        }

        while (remainder[0] == ',')
        {
            remainder = remainder[1..];

            if (IdentifierValidator.Validate(remainder, out remainder)) continue;
            
            remainder = input;
            return false;
        }

        if (remainder[0] != ')')
        {
            remainder = input;
            return false;
        }

        return true;
    }
}
