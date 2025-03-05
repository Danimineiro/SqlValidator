namespace SqlValidator;
public static class SqlStringValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        remaining = input;
        if (input[0] != '\'')
        {
            Console.WriteLine($"String must start with \"'\", but got \"{input[0]}\" instead.");
            return false; 
        }

        if (input.Length < 2)
        {
            Console.WriteLine($"String must end with \"'\", but got nothing instead.");
            return false; 
        }

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] != '\'')
                continue;

            // end of input
            if (++i >= input.Length) 
            {
                remaining = [];
                return true; 
            }

            // escaped apostrophe
            if (input[i] == '\'')
                continue;

            // end of string
            remaining = input[i..];
            return true;
        }

        Console.WriteLine($"String must end with \"'\", but got nothing instead.");
        return false;
    }
}
