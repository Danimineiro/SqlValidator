using SqlValidator.DDLStatements.GeneralItemValidators;
using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements.ProcedureItemValidators;
public class ProcedureParameterValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        ReadOnlySpan<char> afterIdentifier;
        if (Helper.GetNextWord(input, out ReadOnlySpan<char> ioiv, out ReadOnlySpan<char> afterIoiv) &&
            (ioiv.SqlEquals("in") || ioiv.SqlEquals("out") || ioiv.SqlEquals("inout") || ioiv.SqlEquals("variadic")))
        {
            if (!IdentifierValidator.Validate(afterIoiv, out afterIdentifier))
            {
                rest = input;
                Error("Could not validate identifier.");
                return false;
            }
        }
        else
        {
            if (!IdentifierValidator.Validate(input, out afterIdentifier))
            {
                rest = input;
                Error("Could not validate identifier.");
                return false;
            }
        }
        if (!DataTypeValidator.Validate(afterIdentifier, out ReadOnlySpan<char> afterType))
        {
            rest = input;
            Error("Could not validate data type.");
            return false;
        }
        bool result = Helper.GetNextWord(afterType, out ReadOnlySpan<char> word, out ReadOnlySpan<char> afterWord);
        if (result && word.SqlEquals("not"))
        {
            if (!Helper.GetNextWord(afterWord, out ReadOnlySpan<char> @null, out afterWord) || !@null.SqlEquals("null"))
            {
                rest = input;
                Error("Missing 'NULL' after 'NOT'.");
                return false;
            }
            result = Helper.GetNextWord(afterWord, out word, out afterWord);
        }
        if (result && word.SqlEquals("result"))
        {
            result = Helper.GetNextWord(afterWord, out word, out afterWord);
        }
        if (result && word.SqlEquals("default"))
        {
            if (!ExpressionValidator.Validate(afterWord, out afterWord))
            {
                rest = input;
                Error("Could not validate expression");
                return false;
            }
        }
        if (OptionsClauseValidator.Validate(afterWord, out afterWord))
        {
            rest = afterWord;
            return true;
        }
        rest = afterWord;
        return true;
    }

    private static void Error(string error)
    {
        Console.WriteLine("Error in procedure parameter: " + error);
    }
}
