using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterColumnValidator
{
    private static int lengthCovered = 0;
    private const int ALTER_COLUMN_TOKEN_LENGTH = 13;
    private const int ALTER_PARAMETER_TOKEN_LENGTH = 16;
    public static bool Validate(ReadOnlySpan<char> command)
    {

        if (command.StartsWith("ALTER COLUMN"))
        {
            lengthCovered += ALTER_COLUMN_TOKEN_LENGTH;
        }
        else if (command.StartsWith("ALTER PARAMETER"))
        {
            lengthCovered += ALTER_PARAMETER_TOKEN_LENGTH;
        }
        else
        {
            return false;
        }
        if (IdentifierValidator.Validate(command[lengthCovered..]))
        {
            lengthCovered += AlterValidator.GetNextTokenLength(command[lengthCovered..]);
            if (AlterOptionsValidator.Validate(command[lengthCovered..]))
            {
                return true;
            }
        }
        return false;
    }
}
