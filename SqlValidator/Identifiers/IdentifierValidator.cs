namespace SqlValidator.Identifiers;

public static class IdentifierValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (!QuotedIdValidator.Validate(input, out remaining)) return false;

        if (remaining.IsEmpty) return true;

        while (!remaining.IsEmpty && !remaining.IsWhiteSpace())
        {
            if (remaining[0] != '.') return false;

            if (!QuotedIdValidator.Validate(remaining[1..], out remaining)) return false;
        }

        return true;
    }
}
