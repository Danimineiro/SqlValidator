namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class ExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        return ConditionValidator.Validate(input, out rest);
    }
}
