using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class CommonValueExpressionValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
<numeric value expression>
(
    ( <double_amp_op> | <concat_op> )
    <numeric value expression>
)*
#endif
        remaining = input;
#warning CommonValueExpressionValidator has not been implemented yet.
        return false;
    }
}
