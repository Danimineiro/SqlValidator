namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanPrimaryValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
#if false
(
  <common value expression> (
    <between predicate> |
    <match predicate> |
    <like regex predicate> |
    <in predicate> |
    <is null predicate> |
    <quantified comparison predicate> |
    <comparison predicate>
  )?
) |
<exists predicate> |
<xml query>
#endif
        rest = input;
#warning BooleanPrimaryValidator has not been implemented yet.
        return false;
    }
}
