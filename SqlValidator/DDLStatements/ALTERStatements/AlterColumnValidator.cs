using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterColumnValidator
{
    public static bool Validate(ReadOnlySpan<char> command, out String errorLine)
    {
        ReadOnlySpan<char> remainingCommand = command;
        ReadOnlySpan<char> remaining = [];
        ReadOnlySpan<char> nextToken;
        errorLine = "";
        if (Helper.HasNextToken(remainingCommand, "ALTER", out remaining))
        {
            remainingCommand = remaining;
            if (Helper.HasNextToken(remainingCommand, "COLUMN", out remaining))
            {
                remainingCommand = remaining;
            }
            else if (Helper.HasNextToken(remainingCommand, "PARAMETER", out remaining))
            {
                remainingCommand = remaining;
            }
            else
            {
                Helper.TryGetNextToken(remainingCommand, out nextToken);
                errorLine = "Expected COLUMN or PARAMETER but got: " + nextToken.ToString();
                return false;
            }
        }
        else
        {
            Helper.TryGetNextToken(remainingCommand, out nextToken);
            errorLine = "Expected ALTER but got: " + nextToken.ToString();
            return false;
        }
        if (Helper.IsNextTokenIdentifier(remainingCommand, out remaining))
        {
            remainingCommand = remaining;
            if (AlterOptionsValidator.Validate(remainingCommand, out errorLine))
            {
                return true;
            }
        }
        return false;
    }
}
