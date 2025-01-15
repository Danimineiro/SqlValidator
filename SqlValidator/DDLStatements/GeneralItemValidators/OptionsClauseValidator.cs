namespace SqlValidator.DDLStatements.GeneralItemValidators;
internal class OptionsClauseValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> rest)
    {
        rest = input;
        return true;
    }
}
