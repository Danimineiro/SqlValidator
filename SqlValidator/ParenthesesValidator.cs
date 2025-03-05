namespace SqlValidator;
public static class ParenthesesValidator
{
    public static bool StartsAndEndsWithParentheses(ReadOnlySpan<char> command) {
        if(command.StartsWith("(") && command.EndsWith(")"))
        {
            return true;
        }
        return false;
    }
}
