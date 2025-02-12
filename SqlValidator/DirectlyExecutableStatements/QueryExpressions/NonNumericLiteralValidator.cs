
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class NonNumericLiteralValidator
{
    private static readonly string[] ValidTokens = ["FALSE", "TRUE", "UNKNOWN", "NULL"];
    private static readonly string[] ValidTimeTokens = ["DATE", "TIME", "TIMESTAMP"];

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (SqlStringValidator.Validate(input, out remainder)) return true;
        if (BinaryStringLiteralValidator.Validate(input, out remainder)) return true;
        if (input.HasAnyNextToken(ValidTokens, out remainder)) return true;

        if (EscapedTypeValidator.Validate(input, out remainder))
        {
            if (SqlStringValidator.Validate(remainder, out remainder) && RBraceValidator.Validate(remainder, out remainder)) return true;

            remainder = input;
            return false;
        }

        if (!input.HasAnyNextToken(ValidTimeTokens, out remainder)) return false;
        return SqlStringValidator.Validate(remainder, out remainder);
    }
}
