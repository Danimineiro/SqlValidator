
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ApproximateNumericLiteralValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!UnsignedIntegerLiteralValidator.Validate(input, out remainder)) return false;

        if (char.ToLower(remainder[0]) != 'e')
        {
            remainder = input;
            return false;
        }

        //Remove plus or minus if present
        PlusOrMinusValidator.Validate(input, out remainder);

        if (UnsignedIntegerLiteralValidator.Validate(input, out remainder)) return true;

        remainder = input;
        return false;
    }
}
