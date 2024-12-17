namespace SqlValidator;
internal static class Helper
{
    internal static bool SqlStartsWith(this string command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);

    internal static bool SqlStartsWith(this ReadOnlySpan<char> command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);
}
