using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterColumnValidator
{
    private static int lengthCovered = 0;
    private const int ALTER_COLUMN_token_length = 13;
    private const int ALTER_PARAMETER_token_length = 16;
    public static bool Validate(ReadOnlySpan<char> command)
    {

        if (command.StartsWith("ALTER COLUMN") || command.StartsWith("ALTER PARAMETER"))
        {

        }
        return false;
    }
}
