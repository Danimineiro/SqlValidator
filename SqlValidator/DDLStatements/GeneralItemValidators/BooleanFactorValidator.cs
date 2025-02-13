﻿namespace SqlValidator.DDLStatements.GeneralItemValidators;
public class BooleanFactorValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        ReadOnlySpan<char> afterPrimary;
        if (Helper.HasNextSqlWord(input, out ReadOnlySpan<char> afterNot, "not"))
        {
            if (!BooleanPrimaryValidator.Validate(afterNot, out afterPrimary))
            {
                rest = input;
                Error("Could not validate boolean primary");
                return false;
            }
            rest = afterPrimary;
            return true;
        }
        else if (!BooleanPrimaryValidator.Validate(input, out afterPrimary))
        {
            rest = input;
            Error("Could not validate boolean primary");
            return false;
        }
        rest = afterPrimary;
        return true;
    }

    private static void Error(string error)
    {
        Console.WriteLine("Error in boolean factor: " + error);
    }
}
