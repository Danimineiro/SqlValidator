
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ParameterReferenceValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (input[0] == '?')
        {
            remainder = input[1..];
            return true;
        }

        if (input[0] == '$')
        {
            remainder = input[1..];
            if (UnsignedIntegerValidator.Validate(remainder, out remainder)) return true;
        }

        remainder = input;
        return false;
    }
}
