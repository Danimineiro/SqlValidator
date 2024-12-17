namespace SqlValidator.DDLStatements;
public static class OptionNamespaceValidator
{
    /// <summary>
    ///     Includes a terminating space
    /// </summary>
    private const int NAMESPACE_TokenLength = 10;

    /// <summary>
    ///     Includes a terminating space
    /// </summary>
    private const int AS_TokenLength = 3;

    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (!input.StartsWith("NAMESPACE", StringComparison.OrdinalIgnoreCase))
        { 
            return false; 
        }

        if (!SqlStringValidator.Validate(input[NAMESPACE_TokenLength..], out ReadOnlySpan<char> remaining)) 
        { 
            return false; 
        }

        // Skip whitespace after string
        if (!(remaining = remaining[1..]).StartsWith("AS", StringComparison.OrdinalIgnoreCase)) 
        { 
            return false; 
        }

        return IdentifierValidator.Validate(remaining[AS_TokenLength..]);
    }
}
