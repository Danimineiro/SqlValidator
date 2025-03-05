using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.GeneralItemValidators;
public static class TermValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
<value expression primary> ( <star or slash> <value expression primary> )*
#endif
        remaining = input;
#warning TermValidator has not been implemented yet.
        return false;
    }
}
