namespace SqlValidator.DDLStatements;
public static class OptionNamespaceValidator
{
    /// <summary>
    ///     Includes a terminating space
    /// </summary>
    private const int SET_TokenLength = 4;

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
        ReadOnlySpan<char> remaining;

        if (!input.StartsWith("SET", StringComparison.OrdinalIgnoreCase)) return false;
        if (!(remaining = input[SET_TokenLength..]).StartsWith("NAMESPACE", StringComparison.OrdinalIgnoreCase)) return false;

        if (!SqlStringValidator.Validate(remaining[NAMESPACE_TokenLength..], out ReadOnlySpan<char> after)) return false;

        // Skip whitespace after string
        if (!(remaining = after[1..]).StartsWith("AS")) return false;
        return IdentifierValidator.Validate(remaining[(AS_TokenLength)..]);
    }
}
