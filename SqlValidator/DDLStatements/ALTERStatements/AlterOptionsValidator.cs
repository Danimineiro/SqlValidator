using SqlValidator.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterOptionsValidator
{
    private static bool allOptionsCovered = false;

    public static bool Validate(ReadOnlySpan<char> command)
    {
        ReadOnlySpan<char> remaining = [];
        ReadOnlySpan<char> remainingCommand = command;

        if (!Helper.HasNextToken(remainingCommand, "OPTIONS", out remaining))
        {
            return false;
        }

        remainingCommand = remaining;
        if (remainingCommand.TrimStart() is not ['(', .., ')'])
        {
            return false;
        }
        while (!allOptionsCovered)
        {
            int remainingLength = remainingCommand.Length - 2;
            remainingCommand = remainingCommand.TrimStart()[1..remainingLength];
            if (CheckForAddSet(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (Helper.isNextTokenIdentifier(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    // NUMERIC OR NON NUMERIC LITERAL
                    if (Helper.isNextTokenNumeric(remainingCommand, out remaining))
                    {
                        remainingCommand = remaining;
                        if (!Helper.HasNextToken(remainingCommand, ",", out remaining))
                        {
                            remainingCommand = remaining;
                            allOptionsCovered = true;
                        }
                    }
                }

            }
            else if (Helper.HasNextToken(remainingCommand, "DROP", out remaining))
            {
                remainingCommand = remaining;
                if (Helper.isNextTokenIdentifier(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    if (!Helper.HasNextToken(remainingCommand, ",", out remaining))
                    {
                        remainingCommand = remaining;
                        allOptionsCovered = true;
                    }
                }
            }
            else
            {
                return false;
            }

        }
        return false;
    }
    

    internal static bool CheckForAddSet(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken(command, "ADD", out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken(command, "SET", out remaining))
        {
            return true;
        }
        return false;
    }

    
}
