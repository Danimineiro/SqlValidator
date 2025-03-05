namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanValueExpressionValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
        if (!BooleanTermValidator.Validate(input, out ROStr afterTerm))
        {
            rest = input;
            Error("Could not validate boolean term.");
            return false;
        }
        while (Helper.HasNextSqlWord(afterTerm, out ROStr afterOr, "or"))
        {
            if (!BooleanTermValidator.Validate(afterOr, out afterTerm))
            {
                rest = afterTerm;
                Error("Could not validate boolean term.");
                return false;
            }
        }
        rest = afterTerm;
        return true;
    }

    private static void Error(string error)
    {
        Console.WriteLine("Error in boolean value expression: " + error);
    }
}
