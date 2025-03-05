using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterOptionsValidator
{
    private static bool allOptionsCovered = false;

    public static bool Validate(ReadOnlySpan<char> command, out String errorLine)
    {
        ReadOnlySpan<char> remaining = [];
        ReadOnlySpan<char> remainingCommand = command;
        ReadOnlySpan<char> nextToken;
        errorLine = "";

        if (!Helper.HasNextToken(remainingCommand, "OPTIONS", out remaining))
        {
            Helper.TryGetNextToken(remainingCommand, out nextToken);
            errorLine = "Expected 'OPTIONS' but got: " + nextToken.ToString();
            return false;
        }

        remainingCommand = remaining;
        if (remainingCommand.TrimStart() is not ['(', .., ')'])
        {
            errorLine = "Missing either closing or opening parantheses for OPTIONS";
            return false;
        }
        while (!allOptionsCovered)
        {
            int remainingLength = remainingCommand.Length - 1;
            remainingCommand = remainingCommand.TrimStart()[1..remainingLength];
            if (CheckForAddSet(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (Helper.IsNextTokenIdentifier(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    if (Helper.IsNextTokenNumeric(remainingCommand, out remaining) || Helper.IsNextTokenNonNumericLiteral(remainingCommand, out remaining))
                    {
                        remainingCommand = remaining;
                        if (!Helper.HasNextToken(remainingCommand, ",", out remaining))
                        {
                            remainingCommand = remaining;
                            allOptionsCovered = true;
                        }
                    } else
                    {
                        Helper.TryGetNextToken(remainingCommand, out nextToken);
                        errorLine = "Expected a numeric token but got: " + nextToken.ToString();
                        return false;
                    }
                } else
                {
                    Helper.TryGetNextToken(remainingCommand, out nextToken);
                    errorLine = "Expected an identifier but got: " + nextToken.ToString();
                    return false;
                }

            }
            else if (Helper.HasNextToken(remainingCommand, "DROP", out remaining))
            {
                remainingCommand = remaining;
                if (Helper.IsNextTokenIdentifier(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                    if (!Helper.HasNextToken(remainingCommand, ",", out remaining))
                    {
                        remainingCommand = remaining;
                        allOptionsCovered = true;
                    }
                } else
                {
                    Helper.TryGetNextToken(remainingCommand, out nextToken);
                    errorLine = "Expected an identifier but got: " + nextToken.ToString();
                    return false;
                }
            }
            else
            {
                Helper.TryGetNextToken(remainingCommand, out nextToken);
                errorLine = "Expected 'ADD', 'SET' or 'DROP' but got: " + nextToken.ToString();
                return false;
            }

        }
        errorLine = "";
        return true;
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
