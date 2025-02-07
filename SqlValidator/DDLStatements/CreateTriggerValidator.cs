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

        if (!remaining.TrimStart().HasNextToken("ON", out remaining))
        {
            Console.WriteLine("Error: Missing 'ON' keyword.");
            return false;
        }
        remaining = remaining.TrimStart();

        if (!IdentifierValidator.Validate(remaining, out remaining))
        {
            Console.WriteLine("Invalid identifier for trigger target.");
            return false;
        }

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


        if (!remaining.TrimStart().HasNextToken("FOR", out remaining))
        {
            Console.WriteLine("Error: Missing 'FOR' keyword.");
            return false;

        }

        if (!remaining.TrimStart().HasNextToken("EACH", out remaining))
        {
            Console.WriteLine("Error: Missing 'EACH' keyword.");
            return false;

        }

        if (!remaining.TrimStart().HasNextToken("ROW", out remaining))
        {
            Console.WriteLine("Error: Missing 'ROW' keyword.");
            return false;

        }

        if (!remaining.TrimStart().HasNextToken("BEGIN", out remaining))
        {
            Console.WriteLine("Error: Expected 'BEGIN' after 'FOR EACH ROW'.");
            return false;
        }

        if (!remaining.TrimStart().HasNextToken("ATOMIC", out remaining))
        {
            Console.WriteLine("Error: Expected 'ATOMIC' after 'FOR EACH ROW'.");
            return false;
        }

        if (remaining.TrimStart().IndexOf("END", StringComparison.Ordinal) == -1)
        {
            Console.WriteLine("Error: Missing 'END' to close the trigger action'.");
            return false;
        }


        Console.WriteLine("Validation passed!");
        return true;
    }
}
