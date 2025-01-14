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
