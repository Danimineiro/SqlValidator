using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.GeneralItemValidators;
public static class NonNumericLiteralValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
        if (StringValidator.Validate(input, out remaining))
        {
            return true;
        }
        remaining = input;
#warning NonNumericLiteralValidator has not been fully implemented yet.
        return false;
#if false
<string> |
<binary string literal> |
FALSE |
TRUE |
UNKNOWN |
NULL |
( <escaped type> <string> <rbrace> ) |
( ( DATE | TIME | TIMESTAMP ) <string> )
#endif
    }
}
