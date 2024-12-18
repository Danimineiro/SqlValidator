using System.Buffers;

namespace SqlValidator.Identifiers;
public static class QuotedIdValidator
{
    private const char doubleQuote = '"';

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining) => Validate(input, " ", out remaining);

    public static bool Validate(ReadOnlySpan<char> input, ReadOnlySpan<char> allowedContinuations, out ReadOnlySpan<char> remaining)
    {
        if (input[0] == doubleQuote && CheckQuotes(input, out int endIndex))
        {
            remaining = input[(endIndex + 1)..];
            return ID_Validator.Validate(input[1..endIndex]);
        }

        return ID_Validator.Validate(input, out remaining) && (remaining.IsEmpty || allowedContinuations.Contains(remaining[0]));
    }

    private static bool CheckQuotes(ReadOnlySpan<char> input, out int endIndex)
    {
        bool quoted = false;
        endIndex = 0;

        if (input[0] == doubleQuote) quoted = true;
        if (quoted && input.Length < 2)
        {
            return false;
        }

        for (int i = 1; i < input.Length; i++)
        {
            endIndex = i;
            if (input[i] != doubleQuote) continue;

            // end of input
            if (++i >= input.Length)
            {
                return true;
            }

            // escaped apostrophe
            if (input[i] == doubleQuote) continue;

            // end of string
            return true;
        }

        return false;
    }
}
