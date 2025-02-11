
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BooleanTermValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return BooleanFactorValidator.Validate(input, out remainder);

        //TODO: ( AND <boolean factor> )*
    }
}
