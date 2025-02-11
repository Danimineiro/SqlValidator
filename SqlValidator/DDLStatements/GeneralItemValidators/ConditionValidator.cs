namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class ConditionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        return BooleanValueExpressionValidator.Validate(input, out rest);
    }
}
