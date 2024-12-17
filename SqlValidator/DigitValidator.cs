namespace SqlValidator;
public static class DigitValidator
{
    public static bool Validate(char numChar) => numChar is >= '0' and <= '9';

    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty) return false;
        return !input.ContainsAnyExceptInRange('0', '9');
    }
}
