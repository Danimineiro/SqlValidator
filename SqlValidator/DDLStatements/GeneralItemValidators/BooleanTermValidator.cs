namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanTermValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        if (!BooleanFactorValidator.Validate(input, out ReadOnlySpan<char> afterTerm))
        {
            rest = input;
            Error("Could not validate boolean factor.");
            return false;
        }
        while (Helper.HasNextSqlWord(afterTerm, out ReadOnlySpan<char> afterAnd, "and"))
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
