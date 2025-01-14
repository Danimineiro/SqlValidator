using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;

public static class CreateTableValidator
{
    //private const int CREATE_TokenLength = 7;
    //private const int FOREIGN_TABLE_TokenLength = 14;
    //private const int VIRTUAL_VIEW_TokenLength = 14;
    //private const int VIEW_TokenLength = 5;
    //private const int GLOBAL_TEMP_TABLE_TokenLength = 19;
    //private const int AS_TokenLength = 2;

    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (!input.HasNextToken("CREATE", out ReadOnlySpan<char> remaining))
        {
            Console.WriteLine("Error: Missing 'CREATE' keyword.");
            return false;
        }

        if (!remaining.TryGetNextToken(out ReadOnlySpan<char> token))
        {
            Console.WriteLine("Error: Unexpected end of input after 'CREATE'.");
            return false;
        }
        remaining = remaining[(token.Length + 1)..].TrimStart();
        //remaining = remaining.TrimStart();

        switch (token.ToString().ToUpper())
        {
            case "FOREIGN":
                if (!remaining.HasNextToken("TABLE", out remaining))
                {
                    Console.WriteLine("Error: Expected 'TABLE' after 'FOREIGN'");
                    return false;
                }
                break;
            case "VIRTUAL":
                if (!remaining.HasNextToken("VIEW", out remaining))
                {
                    Console.WriteLine("Error: Expected 'VIEW' after 'VIRTUAL'.");
                    return false;
                }
                break;
            case "VIEW":
                //if(!input.HasAnyNextToken(VIEW_TokenLength);
                break;
            case "GLOBAL":
                if (!remaining.HasNextToken("TEMPORARY", out remaining) ||
                    !remaining.HasNextToken("TABLE", out remaining))
                {
                    Console.WriteLine("Error: Expected 'TEMPORARY TABLE' after 'GLOBAL'.");
                    return false;
                }
                break;
            default:
                Console.WriteLine($"Error: Unrecognized token '{token}' after 'CREATE'.");
                return false;
        }



        /*if (remaining.StartsWith("FOREIGN TABLE ", StringComparison.OrdinalIgnoreCase))
        {
            remaining = remaining.Slice(FOREIGN_TABLE_TokenLength);
           //return true;
        }
        else if (remaining.StartsWith("VIRTUAL VIEW ", StringComparison.OrdinalIgnoreCase))
        {
           remaining = remaining.Slice(VIRTUAL_VIEW_TokenLength);
          //return true;
        }
        
        else if (remaining.StartsWith("VIEW ", StringComparison.OrdinalIgnoreCase))
        {
            remaining = remaining.Slice(VIEW_TokenLength);
            //return true;
        }
        
        else if (remaining.StartsWith("GLOBAL TEMPORARY TABLE ", StringComparison.OrdinalIgnoreCase))
        {
            remaining = remaining.Slice(GLOBAL_TEMP_TABLE_TokenLength);
            //return true; 

        }
        else
        {
            return false;
        }*/

        remaining = remaining.TrimStart();

        if (!IdentifierValidator.Validate(remaining, out remaining))
        {
            Console.WriteLine("Error: Invalid identifier for table/view.");
            return false;
        }

        remaining = remaining.TrimStart();



        /* if (!remaining.StartsWith("AS ", StringComparison.OrdinalIgnoreCase))
         {
             return false;

         }*/

        if (token.ToString().ToUpper() == "VIEW" || token.ToString().ToUpper() == "VIRTUAL")
        {
            if (!remaining.HasNextToken("AS", out remaining))
            {
                Console.WriteLine("Error: Missing 'AS' keyword.");
                return false;

            }


            remaining = remaining.Slice(2);
        }
        Console.WriteLine("Validation passed!");
        return true;
    }
}
