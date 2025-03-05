namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class ConditionValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
        return BooleanValueExpressionValidator.Validate(input, out rest);
    }
}
