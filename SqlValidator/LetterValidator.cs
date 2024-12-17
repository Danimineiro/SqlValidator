using System.Buffers;
using System.Runtime.InteropServices;

namespace SqlValidator;
public static class LetterValidator
{
    public static SearchValues<char> SearchValues { get; }

    static LetterValidator()
    {
        char[] chars = Enumerable.Range('a', 26)
            .Concat(Enumerable.Range('A', 26))
            .Concat(Enumerable.Range('\u0153', '\ufffd' - '\u0153'))
            .Select(Convert.ToChar)
            .ToArray();

        SearchValues = System.Buffers.SearchValues.Create(chars);
    }

    public static bool Validate(char c) => SearchValues.Contains(c);

    public static bool ValidateLetters(ReadOnlySpan<char> chars)
    {
        return chars.ContainsAnyExcept(SearchValues);
    }
}
