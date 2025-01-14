using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;

public static class CreateTableValidator
{

    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (!input.HasNextToken("CREATE", out ReadOnlySpan<char> remaining))
        {

            return false;
        }

        if (!remaining.TryGetNextToken(out ReadOnlySpan<char> token))
        {

            return false;
        }
        remaining = remaining[(token.Length + 1)..].TrimStart();

        switch (token.ToString().ToUpper())
        {
            case "FOREIGN":
                if (!remaining.HasNextToken("TABLE", out remaining))
                {
                    return false;
                }
                break;
            case "VIRTUAL":
                if (!remaining.HasNextToken("VIEW", out remaining))
                {

                    return false;
                }
                break;
            case "VIEW":
                break;
            case "GLOBAL":
                if (!remaining.HasNextToken("TEMPORARY", out remaining) ||
                    !remaining.HasNextToken("TABLE", out remaining))
                {

                    return false;
                }
                break;
            default:

                return false;
        }

        remaining = remaining.TrimStart();

        if (!IdentifierValidator.Validate(remaining, out remaining))
        {
            return false;
        }

        remaining = remaining.TrimStart();

        if (token.ToString().ToUpper() == "VIEW" || token.ToString().ToUpper() == "VIRTUAL")
        {
            if (!remaining.HasNextToken("AS", out remaining))
            {
                return false;

            }


            remaining = remaining.Slice(2);
        }
        Console.WriteLine("Validation passed!");
        return true;
    }
}
