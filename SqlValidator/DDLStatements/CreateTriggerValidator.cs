using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;

public static class CreateTriggerValidator
{

    public static bool Validate(ReadOnlySpan<char> input)
    {
        if (!input.HasNextToken("CREATE", out ReadOnlySpan<char> remaining))
        {
            Console.WriteLine("Error: Missing 'CREATE' keyword.");
            return false;
        }

        if (!remaining.TrimStart().HasNextToken("TRIGGER", out remaining))
        {
            Console.WriteLine("Error: Missing 'TRIGGER' keyword.");
            return false;
        }
        Console.WriteLine("first");

        if (!remaining.TrimStart().HasNextToken("ON", out remaining))
        {
            Console.WriteLine("Error: Missing 'ON' keyword.");
            return false;
        }
        Console.WriteLine("second");

        if (!IdentifierValidator.Validate(remaining, out remaining))
        {
            Console.WriteLine("Invalid identifier for trigger target.");
            return false;
        }

        remaining = remaining.TrimStart();

        if (!remaining.TrimStart().HasNextToken("INSTEAD", out remaining))
        {
            Console.WriteLine("Error: Missing 'INSTEAD' keyword.");
            return false;
        }

        if (!remaining.TrimStart().HasNextToken("OF", out remaining))
        {
            Console.WriteLine("Error: Missing 'OF' keyword.");
            return false;
        }

        if (!remaining.TrimStart().HasAnyNextToken(out remaining, "INSERT", "UPDATE", "DELETE"))
        {
            Console.WriteLine("Error: Expected 'INSERT', 'UPDATE', or 'DELETE' after 'INSTEAD OF'."); return false;
        }

        //remaining = remaining.TrimStart();


        if (!remaining.TrimStart().HasNextToken("AS", out remaining))
        {
            Console.WriteLine("Error: Missing 'AS' keyword.");
            return false;

        }


        if (!remaining.TrimStart().HasNextToken("FOR EACH ROW", out remaining))
        {
            Console.WriteLine("Error: Missing 'FOR EACH ROW' keyword.");
            return false;

        }
        Console.WriteLine("Found 'FOR EACH ROW' keyword.");

        remaining = remaining.TrimStart();


        if (!remaining.StartsWith("BEGIN ATOMIC"))
        {
            Console.WriteLine("Error: Expected 'BEGIN ATOMIC' after 'FOR EACH ROW'.");
            return false;
        }
        Console.WriteLine("Found 'BEGIN ATOMIC' keyword.");

        Console.WriteLine("Validation passed!");
        return true;
    }
}
