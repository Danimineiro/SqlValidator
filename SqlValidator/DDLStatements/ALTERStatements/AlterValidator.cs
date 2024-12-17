using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterValidator
{
    /*
    private const int VIRTUAL_TokenLength = 8;
    private const int FOREIGN_TokenLength = 8;
    private const int TABLE_TokenLength = 6;
    private const int VIEW_TokenLength = 5;
    private const int PROCEDURE_TokenLength = 10;
     
    command.StartsWith("VIRTUAL", StringComparison.OrdinalIgnoreCase)
    command.StartsWith("FOREIGN", StringComparison.OrdinalIgnoreCase)
    command.StartsWith("TABLE", StringComparison.OrdinalIgnoreCase
    command.StartsWith("VIEW", StringComparison.OrdinalIgnoreCase)
    command.StartsWith("PROCEDURE", StringComparison.OrdinalIgnoreCase)
    */
    public static bool Validate(ReadOnlySpan<char> command)
    {
        int lengthCovered = 0;
        if(command.StartsWith("VIRTUAL", StringComparison.OrdinalIgnoreCase) || command.StartsWith("FOREIGN", StringComparison.OrdinalIgnoreCase))
        {
            lengthCovered += GetNextTokenLength(command);
            if (CheckForTableViewProcedure(command[lengthCovered..]))
            {
                lengthCovered += GetNextTokenLength(command[lengthCovered..]);
                if(IdentifierValidator.Validate(command[lengthCovered..]))
                {
                    lengthCovered += GetNextTokenLength(command[lengthCovered..]);
                    if(AlterOptionsValidator.Validate(command[lengthCovered..]) || AlterColumnValidator.Validate(command[lengthCovered..]))
                    {
                        return true;
                    }
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
                
        } else if(CheckForTableViewProcedure(command))
        {
            lengthCovered += GetNextTokenLength(command[lengthCovered..]);
            if (IdentifierValidator.Validate(command[lengthCovered..]))
            {
                lengthCovered += GetNextTokenLength(command[lengthCovered..]);
                if (AlterOptionsValidator.Validate(command[lengthCovered..]) || AlterColumnValidator.Validate(command[lengthCovered..]))
                {
                    return true;
                }
            }
        } 
        return false;
    }

    // Counts until next Whitespace and then returns amount of characters
    internal static int GetNextTokenLength(ReadOnlySpan<char> command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            if (command[i..].StartsWith(" ")) {
                return i+1;
            }
        }
        return command.Length;
    }

    private static bool CheckForTableViewProcedure(ReadOnlySpan<char> command) {
        if(command.StartsWith("TABLE", StringComparison.OrdinalIgnoreCase)
            || command.StartsWith("VIEW", StringComparison.OrdinalIgnoreCase)
            || command.StartsWith("PROCEDURE", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
    return false;
    }
}
