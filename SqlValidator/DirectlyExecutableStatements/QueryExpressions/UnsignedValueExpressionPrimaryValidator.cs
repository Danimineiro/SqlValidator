
using SqlValidator.Identifiers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class UnsignedValueExpressionPrimaryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (ParameterReferenceValidator.Validate(input, out remainder)) return true;
        
        //TODO: Too Complicated for now, do later
        //if (EscapedFunctionValidation(input, out remainder)) return true;
        //if (UnescapedFunctionValidator.Validate(input, out remainder)) return true;

        if (IsIdentifier(input, out remainder)) return true;
        if (SubQueryValidator.Validate(input, out remainder)) return true;
        if (NestedExpressionValidator.Validate(input, out remainder)) return true;
        if (SearchedCaseExpressionValidator.Validate(input, out remainder)) return true;

        return CaseExpressionValidator.Validate(input, out remainder);
    }

    private static bool IsIdentifier(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (IdentifierValidator.Validate(input, out remainder)) return true;
        return NonReservedIdentifierValidator.Validate(input, out remainder);
    }

    private static bool EscapedFunctionValidation(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!EscapedFunctionValidator.Validate(input, out remainder)) return false;
        if (!FunctionValidator.Validate(remainder, out remainder)) return false;

        return RBraceValidator.Validate(remainder, out remainder);
    }
}
