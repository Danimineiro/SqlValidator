
using SqlValidator.DirectlyExecutableStatements.QueryExpressions;
using SqlValidator.Identifiers;

namespace SqlValidator;

public static class Helper
{
    public static bool SqlStartsWith(this ReadOnlySpan<char> command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);

    public static bool SqlStartsWith(this ReadOnlySpan<char> command, string start, out ReadOnlySpan<char> remaining)
    {
        if (command.SqlStartsWith(start))
        {
            remaining = command[start.Length..];
            return true;
        }
        remaining = command;
        return false;
    }

    public static bool SqlStartsWithAny(this ReadOnlySpan<char> command, string[] starts)
    {
        foreach (string start in starts)
        {
            if (command.SqlStartsWith(start))
            {
                return true;
            }
        }
        return false;
    }

    public static bool SqlStartsWithAny(this ReadOnlySpan<char> command, string[] starts, out ReadOnlySpan<char> remaining)
    {
        foreach (string start in starts)
        {
            if (command.SqlStartsWith(start))
            {
                remaining = command[start.Length..];
                return true;
            }
        }
        remaining = command;
        return false;
    }

    public static bool SqlEquals(this ReadOnlySpan<char> string1, ReadOnlySpan<char> string2)
        => string1.Equals(string2, StringComparison.OrdinalIgnoreCase);

    public static bool HasAnyNextToken(this ReadOnlySpan<char> input, ReadOnlySpan<string> tokens, out ReadOnlySpan<char> remaining)
        => HasAnyNextToken(input, out remaining, tokens);

    public static bool HasAnyNextToken(this ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining, params ReadOnlySpan<string> tokens)
    {
        if (tokens.IsEmpty)
        {
            remaining = input;
            return !input.IsWhiteSpace();
        }

        ReadOnlySpan<char> longestResult = [];
        for (int i = 0; i < tokens.Length; i++)
        {
            if (input.HasNextToken(tokens[i], out ReadOnlySpan<char> _) && tokens[i].Length > longestResult.Length)
            {
                longestResult = tokens[i];
            }
        }

        if (longestResult.IsEmpty)
        {
            remaining = input;
            return false;
        }

        remaining = input[longestResult.Length..];
        return true;
    }

    /// <summary>
    ///     Check if the given <paramref name="input"/>'s first token is equivalent the same as <paramref name="token"/>, ignoring capitalization.
    ///     If the <paramref name="token"/> is present, returns <c>true</c>, removing and leading whitespace and the given <paramref name="token"/> from the start of <paramref name="input"/> as <paramref name="remaining"/>
    /// </summary>
    /// <param name="input">the string to check</param>
    /// <param name="token">the token to search</param>
    /// <param name="remaining"><paramref name="input"/>, if <paramref name="token"/> was not present, otherwise <paramref name="input"/> with <paramref name="token"/> removed from the start</param>
    /// <returns><c>true</c> if the <paramref name="token"/> could be found, <c>false</c> otherwise.</returns>
    public static bool HasNextToken(this ReadOnlySpan<char> input, ReadOnlySpan<char> token, out ReadOnlySpan<char> remaining)
    {
        if (TryGetNextToken(input, out ReadOnlySpan<char> next))
        {
            remaining = input.TrimStart()[next.Length..].TrimStart();
            return next.Equals(token, StringComparison.OrdinalIgnoreCase);
        }

        remaining = input;
        return false;
    }

    public static bool IsNextTokenNumeric(this ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (TryGetNextToken(input, out ReadOnlySpan<char> next))
        {
            remaining = input.TrimStart()[next.Length..];
            return float.TryParse(next, out _);
        }

        remaining = [];
        return false;
    }

    // Not Implemented
    public static bool IsNextTokenNonNumericLiteral(this ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (TryGetNextToken(input, out ReadOnlySpan<char> next))
        {
            remaining = input.TrimStart()[next.Length..];
            return NonNumericLiteralValidator.Validate(next, out _);
        }

        remaining = [];
        return false;
    }

    internal static bool IsNextTokenIdentifier(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (TryGetNextToken(input, out ReadOnlySpan<char> next))
        {
            remaining = input.TrimStart()[next.Length..];
            return IdentifierValidator.Validate(next, out _);
        }

        remaining = [];
        return false;
    }

    public static bool TryGetNextToken(this ReadOnlySpan<char> input, out ReadOnlySpan<char> token)
    {
        input = input.TrimStart();
        if (input.Length < 2)
        {
            token = input;
            return true;
        }

        switch (input[0])
        {
            case char character when character is '\'' or '"':

                for (int i = 1; i < input.Length; i++)
                {
                    if (input[i] != character) continue;

                    if (++i < input.Length)
                    {
                        if (input[i] == character) continue;
                        if (!char.IsWhiteSpace(input[i]))
                        {
                            token = [];
                            return false;
                        }
                        if (input[i] == ',')
                        {
                            if(i > 1)
                            {
                                token = input[..(i-1)];
                                return true;
                            } else
                            {
                                token = [];
                                return false;
                            }
                        }
                    }

                    token = input[..i];
                    return true;
                }

                // Bad input
                token = [];
                return false;

            case '[':
                token = input[..(input.IndexOf(']') + 1)];
                return true;

            default:
                int tokenEnd = input.IndexOf(' ');

                if (tokenEnd == -1)
                {
                    token = input;
                    return true;
                }

                token = input[..tokenEnd];
                return true;
        }
    }

    public static int IntLog10(int x)
    {
        if (x < 0)
            return -1;

        int log = 0;
        for (; x > 0; x /= 10, log++) ;
        return log;
    }

    public static uint UIntLog10(uint x)
    {
        uint log = 0;
        for (; x > 0; x /= 10, log++) ;
        return log;
    }

    public static bool GetNextWord(ReadOnlySpan<char> input, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest)
    {
        ReadOnlySpan<char> temp = input.TrimStart();
        if (temp.Length == 0)
        {
            word = "";
            rest = "";
            return false;
        }    
        switch(temp[0])
        {
            case '\'':
                return GetNextQuote('\'', input, temp, out word, out rest);

            case '"':
                return GetNextQuote('"', input, temp, out word, out rest);

            default:
                break;
        }
        if (IsWordChar(temp[0]))
        {
            int i = 1;
            for (; i < temp.Length; i++)
            {
                if (!IsWordChar(temp[i]))
                {
                    word = temp[..i];
                    rest = temp[i..];
                    return true;
                }
            }
            word = temp;
            rest = "";
            return true;
        }
        // special char
        word = temp[..1];
        rest = temp[1..];
        return true;
    }

    private static bool GetNextQuote(char quoteChar, ReadOnlySpan<char> input, ReadOnlySpan<char> temp, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest)
    {
        int i = 1;
        for (; i < temp.Length; i++)
        {
            if (temp[i] == quoteChar)
            {
                if (i == temp.Length - 1)
                {
                    word = temp;
                    rest = string.Empty;
                    return true;
                }
                if (temp[i - 1] == '\\' || temp[++i] == quoteChar)
                {
                    continue;
                }
                word = temp[..i];
                rest = temp[i..];
                return true;
            }
        }
        if (i == temp.Length)
        {
            word = string.Empty;
            rest = input;
            return false;
        }
        word = temp[..i];
        rest = temp[i..];
        return true;
    }

    private static bool IsWordChar(this char c)
    {
        return LetterValidator.Validate(c) || DigitValidator.Validate(c) || c == '_';
    }
}
