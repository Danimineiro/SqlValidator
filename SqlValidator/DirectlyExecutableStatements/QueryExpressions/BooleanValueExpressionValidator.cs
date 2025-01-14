
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BooleanValueExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return BooleanTermValidator.Validate(input, out remainder);

        //TODO: ( OR <boolean term> )*
    }
}
