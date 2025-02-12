using SqlValidator.DDLStatements.GeneralItemValidators;
using SqlValidator.DDLStatements.ProcedureItemValidators;
using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;
public class CreateProcedureValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        if (!Helper.HasNextSqlWord(command, out ReadOnlySpan<char> rest, "create"))
        {
            return false;
        }
        if (!Helper.GetNextWord(rest, out ReadOnlySpan<char> word, out rest))
        {
            return false;
        }
        if (word.SqlEquals("virtual") || word.SqlEquals("foreign"))
        {
            if (!Helper.GetNextWord(rest, out word, out rest) || !word.SqlEquals("procedure") && !word.SqlEquals("function"))
            {
                return false;
            }
        }
        else if (!word.SqlEquals("procedure") && !word.SqlEquals("function"))
        {
            return false;
        }
        if (!QuotedIdValidator.Validate(rest, out rest))
        {
            return false;
        }
        if (!Helper.HasNextSpecialChar(rest, out rest, '('))
        {
            return false;
        }
        if (ProcedureParameterValidator.Validate(rest, out rest))
        {
            while (Helper.HasNextSpecialChar(rest, out rest, ','))
            {
                if (!ProcedureParameterValidator.Validate(rest, out rest))
                {
                    return false;
                }
            }
        }
        if (!Helper.HasNextSpecialChar(rest, out rest, ')'))
        {
            return false;
        }
        if (!Helper.HasNextSqlWord(rest, out rest, "returns"))
        {
            return ValidateEndOfCommand(rest);
        }
        OptionsClauseValidator.Validate(rest, out rest);
        if (DataTypeValidator.Validate(rest, out rest))
        {
            return ValidateEndOfCommand(rest);
        }
        Helper.HasNextSqlWord(rest, out rest, "table");
        if (!Helper.HasNextSpecialChar(rest, out rest, '('))
        {
            return false;
        }
        if (!ProcedureResultColumnValidator.Validate(rest, out rest))
        {
            return false;
        }
        while (Helper.HasNextSpecialChar(rest, out rest, ','))
        {
            if (!ProcedureResultColumnValidator.Validate(rest, out rest))
            {
                return false;
            }
        }
        if (!Helper.HasNextSpecialChar(rest, out rest, ')'))
        {
            return false;
        }
        return ValidateEndOfCommand(rest);
    }

    private static bool ValidateEndOfCommand(ReadOnlySpan<char> input)
    {
        if (string.IsNullOrWhiteSpace(input.ToString()))
        {
            return true;
        }
        else
        {
            return Helper.HasNextSpecialChar(input, out ReadOnlySpan<char> rest, ';') && string.IsNullOrWhiteSpace(rest.ToString());
        }
    }
}
