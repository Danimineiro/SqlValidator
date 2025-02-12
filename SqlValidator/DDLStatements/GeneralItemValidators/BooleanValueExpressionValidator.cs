namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanValueExpressionValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        if (!BooleanTermValidator.Validate(input, out ReadOnlySpan<char> afterTerm))
        {
            rest = input;
            Error("Could not validate boolean term.");
            return false;
        }
        while (Helper.HasNextSqlWord(afterTerm, out ReadOnlySpan<char> afterOr, "or"))
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
