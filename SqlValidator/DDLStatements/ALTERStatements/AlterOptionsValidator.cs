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

        if (!Helper.HasNextToken("OPTIONS", remainingCommand, out remaining)
        {
            return false;
        }

        remainingCommand = remaining;
        if (remainingCommand is not ['(', .., ')'])
        {
            return false;
        }
        while (!allOptionsCovered)
        {
            remainingCommand = remainingCommand[1..];
            if (CheckForAddSet(remainingCommand, out remaining))
            {
                remainingCommand = remaining;
                if (IdentifierValidator.Validate(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                }

            }
            else if (Helper.HasNextToken(remainingCommand, "DROP", out remaining))
            {
                remainingCommand = remaining;
                if (IdentifierValidator.Validate(remainingCommand, out remaining))
                {
                    remainingCommand = remaining;
                }
            }
        }
        /*
        {
            
            if (IdentifierValidator.Validate(command[lengthCovered..tokenEnd], out _))
            {
                lengthCovered += AlterValidator.GetNextTokenLength(command[lengthCovered..]);
                ReadOnlySpan<char> nextToken = AlterValidator.GetNextToken(command[lengthCovered..].TrimStart());
                bool tokenIsNumeric = float.TryParse(nextToken, out _);
                if (!tokenIsNumeric)
                {
                    lengthCovered += nextToken.Length;
                    if (!command[lengthCovered..].TrimStart().StartsWith(","))
                    {
                        allOptionsCovered = true;
                    }
                }
                else
                {
                    lengthCovered += nextToken.Length;
                    if (!command[lengthCovered..].TrimStart().StartsWith(","))
                    {
                        allOptionsCovered = true;
                    }
                }
            }

        }
        else if (command[lengthCovered..].StartsWith("DROP"))
        {
            lengthCovered += DROP_TOKEN_LENGTH;
            if (IdentifierValidator.Validate(command[lengthCovered..], out _))
            {
                lengthCovered += AlterValidator.GetNextTokenLength(command[lengthCovered..]);
                if (!command[lengthCovered..].TrimStart().StartsWith(","))
                {
                    allOptionsCovered = true;
                }
            }
        }
        else
        {
            return false;
        }
        */

        return true;
    }

    internal static bool CheckForAddSet(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.HasNextToken("ADD", command, out remaining))
        {
            return true;
        }
        if (Helper.HasNextToken("SET", command, out remaining))
        {
            return true;
        }
        return false;
    }
}
