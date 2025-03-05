namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class ExpressionValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
        return ConditionValidator.Validate(input, out rest);
    }
}
