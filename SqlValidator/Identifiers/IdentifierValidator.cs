namespace SqlValidator.Identifiers;

public static class IdentifierValidator
{
    private const string AllowedContinuations = " .";

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (!QuotedIdValidator.Validate(input, AllowedContinuations, out remaining)) return false;

        if (remaining.IsEmpty) return true;

        while (!remaining.IsEmpty && !remaining.IsWhiteSpace())
        {
            if (remaining[0] != '.') break;
            
            if (!QuotedIdValidator.Validate(remaining.TrimStart('.'), AllowedContinuations, out remaining)) return false;
        }

        return true;
    }
}
