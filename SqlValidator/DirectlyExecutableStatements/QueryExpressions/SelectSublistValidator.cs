
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class SelectSublistValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        return SelectDerivedColumnValidator.Validate(input, out remainder);

        //TODO: All in group
    }
}
