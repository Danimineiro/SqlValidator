using System.Runtime.CompilerServices;

namespace SqlValidator;
internal static class Helper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool SqlStartsWith(this string command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool SqlStartsWith(this ReadOnlySpan<char> command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);
}
