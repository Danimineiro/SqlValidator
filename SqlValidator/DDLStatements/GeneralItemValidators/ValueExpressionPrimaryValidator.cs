namespace SqlValidator.DDLStatements.GeneralItemValidators;
public static class ValueExpressionPrimaryValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
<non numeric literal> |
(
    ( <plus or minus> )?
    (
        <unsigned numeric literal> |
        (
            <unsigned value expression primary>
            (
                <lsbrace>
                <numeric value expression>
                <rsbrace>
            )*
        )
    )
)
#endif
        remaining = input;
#warning ValueExpressionPrimaryValidator has not been implemented yet.
        return false;
    }
}
