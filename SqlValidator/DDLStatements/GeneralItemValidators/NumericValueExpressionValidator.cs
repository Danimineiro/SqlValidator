namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class NumericValueExpressionValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
<term> ( <plus or minus> <term> )*
#endif
        remaining = input;
#warning NumericValueExpressionValidator has not been implemented yet.
        return false;
    }
}
