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
        ReadOnlySpan<char> nextToken;
        String errorLine;
        if (CheckForVirtualOrForeign(remainingCommand, out remaining))
        {
            remainingCommand = remaining;
            if (CheckForTableViewProcedure(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (Helper.IsNextTokenIdentifier(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    if (AlterColumnValidator.Validate(remainingCommand, out errorLine) || AlterOptionsValidator.Validate(remainingCommand, out errorLine))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(errorLine);
                        return false;
                    }
                }
                else
                {
                    Helper.TryGetNextToken(remainingCommand, out nextToken);
                    Console.WriteLine("Expected an identifier but got " + nextToken.ToString());
                    return false;
                }
            }
            else
            {

                Helper.TryGetNextToken(remainingCommand, out nextToken);
                Console.WriteLine("Expected 'TABLE', 'VIEW' OR 'PROCEDURE' but got " + nextToken.ToString());
                return false;
            }

        }
        else if (CheckForTableViewProcedure(remainingCommand, out remaining))
        {
            remainingCommand = remaining;
            if (Helper.IsNextTokenIdentifier(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (AlterColumnValidator.Validate(remainingCommand, out errorLine) || AlterOptionsValidator.Validate(remainingCommand, out errorLine))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(errorLine);
                    return false;
                }
            }
        }
        Helper.TryGetNextToken(remainingCommand, out nextToken);
        Console.WriteLine("Expected 'TABLE', 'VIEW' OR 'PROCEDURE' but got " + nextToken.ToString());
        return false;
    }

    private static bool CheckForTableViewProcedure(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken(command, "TABLE", out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken(command, "PROCEDURE", out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken(command, "VIEW", out remaining))
        {
            return true;
        }
        return false;
    }
    private static bool CheckForVirtualOrForeign(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken(command, "VIRTUAL", out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken(command, "FOREIGN", out remaining))
        {
            return true;
        }
        return false;
    }

}
