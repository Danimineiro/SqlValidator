using SqlValidator.DirectlyExecutableStatements.QueryExpressions;
using SqlValidator.DirectlyExecutableStatements.QueryExpressions.With;

namespace SqlValidator.DirectlyExecutableStatements;
public static class DirectlyExecutableStatementValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        Span<Range> ranges = new Range[2];
        command.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries);

        Span<char> firstToken = new char[ranges[0].End.Value];
        command[ranges[0]].ToLowerInvariant(firstToken);

        return firstToken switch
        {
            "select" => QueryExpressionBodyValidator.Validate(command, out _),
            "with" => WithValidator.Validate(command),
            _ => false
        };

    }
}
