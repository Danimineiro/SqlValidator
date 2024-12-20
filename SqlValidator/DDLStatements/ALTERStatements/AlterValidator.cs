using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        ReadOnlySpan<char> remainingCommand = command;
        ReadOnlySpan<char> remaining = [];
        if (CheckForVirtualOrForeign(remainingCommand, out remaining))
        {
            remainingCommand = remaining;
            if (CheckForTableViewProcedure(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (IdentifierValidator.Validate(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    if (AlterOptionsValidator.Validate(remainingCommand) || AlterColumnValidator.Validate(remainingCommand))
                    {
                        return true;
                    }
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

        }
        else if (CheckForTableViewProcedure(command, out remaining))
        {
            remainingCommand = remaining;
            if (IdentifierValidator.Validate(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (AlterOptionsValidator.Validate(remainingCommand) || AlterColumnValidator.Validate(remainingCommand))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool CheckForTableViewProcedure(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken("TABLE", command, out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken("FOR", command, out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken("VIEW", command, out remaining))
        {
            return true;
        }
        return false;
    }
    private static bool CheckForVirtualOrForeign(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken("VIRTUAL", command, out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken("FOREIGN", command, out remaining))
        {
            return true;
        }
        return false;
    }
}
