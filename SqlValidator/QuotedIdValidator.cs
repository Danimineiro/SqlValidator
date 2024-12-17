using System.Buffers;

namespace SqlValidator;
public static class QuotedIdValidator
{
    private static readonly SearchValues<char> Letters = SearchValues.Create('A');

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        bool quoted = false;

        if (input[0] == '"' && CheckQuotes(input, out int length)) 
        {
            remaining = input[length..];
            return true;
        }

        //TODO: Missing letter validation
        remaining = input;
        return true;
    }

    private static bool CheckQuotes(ReadOnlySpan<char> input, out int length)
    {
        bool quoted = false;
        length = 0;

        if (input[0] == '"') quoted = true;
        if (quoted && input.Length < 2)
        {
            return false;
        }

        for (int i = 1; i < input.Length; i++)
        {
            length = i + 1;
            if (input[i] != '"') continue;

            // end of input
            if (++i >= input.Length)
            {
                return true;
            }

            // escaped apostrophe
            if (input[i] == '"') continue;

            // end of string
            return true;
        }

        return false;
    }
}
