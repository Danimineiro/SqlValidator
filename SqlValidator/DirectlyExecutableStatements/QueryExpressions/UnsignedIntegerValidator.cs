
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnsignedIntegerValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return UnsignedIntegerLiteralValidator.Validate(input, out remainder);
    }
}
