namespace SqlValidator.DDLStatements;

public static class DDLStatementValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        Span<Range> ranges = new Range[2];
        command.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries);

        return command[ranges[0]] switch
        {
            "Create" => true,
            "Alter" => true,
            "Set" => true,
            _ => false
        };
    }
}
