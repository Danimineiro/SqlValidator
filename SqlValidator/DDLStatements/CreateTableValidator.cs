namespace SqlValidator.DDLStatements;

public static class CreateTableValidator
{
    private const int CREATETABLE_TokenLength = 13;
    private const int AS_TokenLength = 3;

    public static bool Validate(ReadOnlySpan<char> input)
    {

        if (!input.StartsWith("CREATE TABLE ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!IdentifierValidator.Validate(input[CREATETABLE_TokenLength..], out
        ReadOnlySpan<char> remaining))
        {
            return false;
        }
        if (!(remaining = remaining[1..]).StartsWith("AS ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        return IdentifierValidator.Validate(remaining[AS_TokenLength..]);

    }

}
