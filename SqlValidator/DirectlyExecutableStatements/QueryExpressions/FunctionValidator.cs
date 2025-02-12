
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class FunctionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        Console.WriteLine("Function Validation is currently unsupported.");

        remainder = input;
        return false;
    }
}
