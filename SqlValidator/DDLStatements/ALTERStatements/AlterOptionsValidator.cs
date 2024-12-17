using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterOptionsValidator
{
    private static int lengthCovered = 0;
    public static bool Validate(ReadOnlySpan<char> command)
    {

        if (command.StartsWith("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            lengthCovered += AlterValidator.GetNextTokenLength(command);
            if (ParenthesesValidator.StartsAndEndsWithParentheses(command[lengthCovered..]))
            {
                lengthCovered += 1;
                if (command[lengthCovered..].StartsWith("ADD") || command[lengthCovered..].StartsWith("SET"))
                {
                    lengthCovered += 4;
                }
                else if (command[lengthCovered..].StartsWith("DROP"))
                {
                    lengthCovered += 5;
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
