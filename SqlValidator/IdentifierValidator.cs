namespace SqlValidator;

public static class IdentifierValidator
{
    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (!QuotedIdValidator.Validate(input, out ReadOnlySpan<char> remaining)) return false;

        if (remaining.IsEmpty) return true;

        while (!remaining.IsEmpty && !remaining.IsWhiteSpace())
        {
            if (remaining[0] != '.') return false;

            if (!QuotedIdValidator.Validate(input, out remaining)) return false;
        }

        return true;
    }
}
