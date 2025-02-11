
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class CommonValueExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!NumericValueExpressionValidator.Validate(input, out remainder)) return false;

        while (DoubleAmpOpValidator.Validate(remainder, out remainder) || ConcatOpValidator.Validate(remainder, out remainder))
        {
            if (NumericValueExpressionValidator.Validate(remainder, out remainder)) continue;
            
            remainder = input;
            return false; 
        }

        return true;
    }
}
