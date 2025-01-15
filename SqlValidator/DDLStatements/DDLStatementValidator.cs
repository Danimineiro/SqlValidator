namespace SqlValidator.DDLStatements;

public static class DDLStatementValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        Span<Range> ranges = new Range[3];
        command.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries);

        Span<char> firstToken = new char[ranges[0].End.Value];
        command[ranges[0]].ToLowerInvariant(firstToken);

        return firstToken switch
        {
            "create" => /*CreateTableValidator.Validate(command) || */CreateTriggerValidator.Validate(command),
            "alter" => true,
            "set" => OptionNamespaceValidator.Validate(command[4..]),
            _ => false

        };
    }

    /* private static bool ValidateCreateCommand(ReadOnlySpan<char> command)
     {
         if (command.Slice(0, 13).Equals("CREATE TRIGGER", StringComparison.OrdinalIgnoreCase))
         {
             return CreateTriggerValidator.Validate(command);

         }
         return CreateTableValidator.Validate(command);
     }*/
}
