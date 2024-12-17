namespace SqlValidator.Identifiers;
public static class ID_Validator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (input.IsEmpty) 
        {
            remaining = input;
            return false; 
        }

        char character = input[0];
        if (!(character is '@' or '#' || LetterValidator.Validate(character)))
        {
            remaining = input;
            return false;
        }

        remaining = input[1..];
        for (int i = 0; i < remaining.Length; i++)
        {
            if (remaining[i] == '_') continue;
            if (LetterValidator.Validate(remaining[i])) continue;
            if (DigitValidator.Validate(remaining[i])) continue;

            remaining = remaining[i..];
            return true;
        }

        remaining = [];
        return true;
    }

    public static bool Validate(ReadOnlySpan<char> input) => Validate(input, out ReadOnlySpan<char> remaining) && remaining.IsEmpty;
}
