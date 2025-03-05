namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanTermValidator
{
    public static bool Validate(ROStr input, out ROStr rest)
    {
        if (!BooleanFactorValidator.Validate(input, out ROStr afterTerm))
        {
            rest = input;
            Error("Could not validate boolean factor.");
            return false;
        }
        while (Helper.HasNextSqlWord(afterTerm, out ROStr afterAnd, "and"))
        {
            if (!BooleanFactorValidator.Validate(afterAnd, out afterTerm))
            {
                rest = afterTerm;
                Error("Could not validate boolean factor.");
                return false;
            }
        }
        rest = afterTerm;
        return true;
    }

    private static void Error(string error)
    {
        Console.WriteLine("Error in boolean term: " + error);
    }
}
