using System.Buffers;

namespace SqlValidator;
public static class QuotedIdValidator
{
    private static readonly SearchValues<char> Letters = SearchValues.Create('A');

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        throw new NotImplementedException();

        bool quoted = false;

        if (input[0] == '"') quoted = true;
        if (quoted && input.Length < 2) 
        { 
            remaining = input;
            return false; 
        }

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] != '\'') continue;

            // end of input
            if (++i >= input.Length)
            {
                remaining = [];
                return true;
            }

            // escaped apostrophe
            if (input[i] == '\'') continue;

            // end of string
            remaining = input[i..];
            return true;
        }

        
        //return false;
    }
}
