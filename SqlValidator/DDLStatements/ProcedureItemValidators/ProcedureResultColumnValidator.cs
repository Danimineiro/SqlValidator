using SqlValidator.DDLStatements.GeneralItemValidators;
using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements.ProcedureItemValidators;
internal class ProcedureResultColumnValidator
{
    internal static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remaining)
    {
        if (!IdentifierValidator.Validate(input, out ReadOnlySpan<char> rest))
        {
            remaining = input;
            return false;
        }
        if (!DataTypeValidator.Validate(rest, out rest))
        {
            remaining = input;
            return false;
        }
        if (Helper.HasNextSqlWord(rest, out rest, "not") && !Helper.HasNextSqlWord(rest, out rest, "null"))
        {
            remaining = input;
            return false;
        }
        OptionsClauseValidator.Validate(rest, out rest);
        remaining = rest;
        return true;
    }
}
