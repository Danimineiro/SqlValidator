
using System.Buffers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class HexitValidator
{
    private static SearchValues<char> ValidValues { get; } = SearchValues.Create(['A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);

    public static bool IsValidChar(char v) => ValidValues.Contains(v);
}
