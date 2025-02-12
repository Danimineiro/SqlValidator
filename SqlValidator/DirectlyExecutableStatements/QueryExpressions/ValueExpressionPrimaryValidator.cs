
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class ValueExpressionPrimaryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (NonNumericLiteralValidator.Validate(input, out remainder)) return true;

        //Remove Plus or Minus if present
        PlusOrMinusValidator.Validate(input, out remainder);

        if (UnsignedNumericLiteralValidator.Validate(remainder, out remainder)) return true;
        
        if (UnsignedValueExpressionPrimaryValidator.Validate(remainder, out remainder))
        {
            while(LSBraceValidator.Validate(remainder, out remainder))
            {
                if (NumericValueExpressionValidator.Validate(remainder, out remainder) 
                    && RSBraceValidator.Validate(remainder, out remainder)) continue;

                remainder = input;
                return false;
            }
        }

        return true;
    }
}
