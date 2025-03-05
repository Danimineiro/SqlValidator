namespace SqlValidator.DDLStatements.GeneralItemValidators;
internal class StatementValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
#if false
( ( <identifier> <colon> )? ( <loop statement> | <while statement> | <compound statement> ) )
<if statement> | <delimited statement>
#endif
        rest = input;
#warning StatementValidator has not been implemented yet.
        return false;
    }
}
