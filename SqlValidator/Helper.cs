﻿using static System.Net.Mime.MediaTypeNames;

namespace SqlValidator;
public static class Helper
{
    internal static bool SqlStartsWith(this ReadOnlySpan<char> command, string start)
        => command.StartsWith(start, StringComparison.OrdinalIgnoreCase);

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
            if (input.HasNextToken(tokens[i], out ReadOnlySpan<char> token) && token.Length > longestResult.Length)
            {
                longestResult = tokens[i];
            }
        }

        remaining = input[longestResult.Length..];
        return !longestResult.IsEmpty;
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
            remaining = input[next.Length..];
            return next.Equals(token, StringComparison.OrdinalIgnoreCase);
        }

        remaining = input;
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

    internal static bool SqlStartsWith(this ReadOnlySpan<char> command, string start, out ReadOnlySpan<char> remaining)
    {
        if (command.SqlStartsWith(start))
        {
            remaining = command[start.Length..];
            return true;
        }
        remaining = command;
        return false;
    }

    internal static bool SqlStartsWithAny(this ReadOnlySpan<char> command, string[] starts)
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

    internal static bool SqlStartsWithAny(this ReadOnlySpan<char> command, string[] starts, out ReadOnlySpan<char> remaining)
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
}
