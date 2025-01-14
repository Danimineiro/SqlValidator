using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterColumnValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        ReadOnlySpan<char> remainingCommand = command;
        ReadOnlySpan<char> remaining = [];
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
                return false;
            }
        }
        else
        {
            return false;
        }
        if (Helper.isNextTokenIdentifier(remainingCommand, out remaining))
        {
            remainingCommand = remaining;
            if (AlterOptionsValidator.Validate(remainingCommand))
            {
                return true;
            }
        }
        return false;
    }
}
