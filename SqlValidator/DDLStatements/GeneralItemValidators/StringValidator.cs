using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.GeneralItemValidators;
public static class StringValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
<string literal>
#endif
        return StringLiteralValidator.Validate(input, out remaining);
    }
}
