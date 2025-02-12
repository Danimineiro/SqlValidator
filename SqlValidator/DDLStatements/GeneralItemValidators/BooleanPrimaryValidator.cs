namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanPrimaryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        /*
         * (
         *   <common value expression> (
         *     <between predicate> |
         *     <match predicate> |
         *     <like regex predicate> |
         *     <in predicate> |
         *     <is null predicate> |
         *     <quantified comparison predicate> |
         *     <comparison predicate>
         *   )?
         * ) |
         * <exists predicate> |
         * <xml query>
         */
        rest = input;
        return true;
    }
}
