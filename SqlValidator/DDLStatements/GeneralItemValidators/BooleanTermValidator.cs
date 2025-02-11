using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        while (Helper.GetNextWord(afterTerm, out ReadOnlySpan<char> @or, out ReadOnlySpan<char> afterOr) && @or.SqlEquals("or"))
        {
            if (!BooleanFactorValidator.Validate(afterOr, out afterTerm))
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
