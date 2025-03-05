namespace SqlValidator.DDLStatements.GeneralItemValidators;
public static class StringLiteralValidator
{
    public static bool Validate(ROStr input, out ROStr remaining)
    {
#if false
(N | E)?
'
('' | <everything but '>)*
'
#endif
        ROStr rest;
        // ("N" | "E")? "\'"
        if (Helper.HasNextSingleChar(input, out rest, 'N') && !Helper.HasNextSingleChar(rest, out rest, '\'') ||
            Helper.HasNextSingleChar(input, out rest, 'E') && !Helper.HasNextSingleChar(rest, out rest, '\'') ||
            !Helper.HasNextSingleChar(input, out rest, '\''))
        {
            remaining = input;
            return false;
        }
        // ("\'\'" || ~["\'"]) "\'"
        while (rest.Length > 0)
        {
            char c = rest[0];
            if (c == '\'')
            {
                rest = rest[1..];
                if (rest.Length == 0 || rest[0] != '\'')
                {
                    remaining = rest;
                    return true;
                }
            }
            rest = rest[1..];
        }
        remaining = input;
        return false;
    }
}
