using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterOptionsValidator
{
    private const int ADD_SET_TOKEN_LENGTH = 4;
    private const int DROP_TOKEN_LENGTH = 5;
    private static int lengthCovered = 0;
    public static bool Validate(ReadOnlySpan<char> command)
    {
        
        if(command.StartsWith("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            lengthCovered += AlterValidator.GetNextTokenLength(command);
            if(ParenthesesValidator.StartsAndEndsWithParentheses(command[lengthCovered..])) {
                lengthCovered += 1;
                if (command[lengthCovered..].StartsWith("ADD") || command[lengthCovered..].StartsWith("SET"))
                {
                    lengthCovered += ADD_SET_TOKEN_LENGTH;
                } else if(command[lengthCovered..].StartsWith("DROP"))
                {
                    lengthCovered += DROP_TOKEN_LENGTH;
                    if (!IdentifierValidator.Validate(command[lengthCovered..]))
                    {
                        return false;
                    }
                }
            }
        } 
        return false;
    }

    internal static bool ValidateDROPOption(ReadOnlySpan<char> command)
    {
        return false;
    }

    internal static bool ValidateADDSETOption(ReadOnlySpan<char> command)
    {
        return false;
    }
}
