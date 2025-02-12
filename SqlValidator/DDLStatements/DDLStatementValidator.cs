using SqlValidator.DDLStatements.ALTERStatements;

using SqlValidator.DirectlyExecutableStatements;

namespace SqlValidator.DDLStatements;

public static class DDLStatementValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        Span<Range> ranges = new Range[2];
        command.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries);

        Span<char> firstToken = new char[ranges[0].End.Value];
        command[ranges[0]].ToLowerInvariant(firstToken);

        return firstToken switch
        {
            "create" => true,
            "alter" => AlterValidator.Validate(command[6..]),
            "set" => OptionNamespaceValidator.Validate(command[4..]),
            _ => DirectlyExecutableStatementValidator.Validate(command)
        };
    }
}
