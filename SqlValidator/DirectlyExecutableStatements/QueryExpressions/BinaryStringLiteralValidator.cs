
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BinaryStringLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;

        // Smallest BinaryString is X'00' (5 chars)
        if (remainder.Length < 5) return false;

        if (char.ToLower(input[0]) != 'x') return false;
        if (input[1] != '\'') return false;

        remainder = input[2..];
        if (!HexitValidator.IsValidChar(input[0]) || HexitValidator.IsValidChar(input[1]))
        {
            remainder = input;
            return false;
        }

        while (input[0] != '\'')
        {
            if (HexitValidator.IsValidChar(input[0]) && HexitValidator.IsValidChar(input[1])) continue;
            
            remainder = input;
            return false;
        }

        remainder = remainder[1..];
        return true;
    }
}
