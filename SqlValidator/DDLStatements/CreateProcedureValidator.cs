using SqlValidator.DDLStatements.GeneralItemValidators;
using SqlValidator.DDLStatements.ProcedureItemValidators;
using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;
public class CreateProcedureValidator
{
    public static bool Validate(ROStr command)
    {
        if (!Helper.HasNextSqlWord(command, out ROStr rest, "create"))
        {
            return false;
        }
        ValidateVirtualOrForeign(rest, out rest);
        if (!ValidateProcedureOrFunction(rest, out rest))
        {
            return false;
        }
        if (!ValidateProcedureHead(rest, out rest))
        {
            return false;
        }
        if (!ValidateReturnStatement(rest, out rest))
        {
            return false;
        }
        OptionsClauseValidator.Validate(rest, out rest);
        ValidateAsStatement(rest, out rest);
        return ValidateEndOfCommand(rest);
    }

    /// <summary>
    /// ( VIRTUAL | FOREIGN )?
    /// </summary>
    private static bool ValidateVirtualOrForeign(ROStr input, out ROStr rest)
    {
        rest = input;
        if (Helper.GetNextWord(input, out ROStr word, out ROStr rest1))
        {
            if (word.SqlEquals("VIRTUAL") || word.SqlEquals("FOREIGN"))
            {
                rest = rest1;
            }
        }
        return true;
    }

    /// <summary>
    /// ( PROCEDURE | FUNCTION )
    /// </summary>
    private static bool ValidateProcedureOrFunction(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!Helper.GetNextWord(input, out ROStr word, out ROStr rest1))
        {
            Console.WriteLine($"Expected \"PROCEDURE\" or \"FUNCTION\"");
            return false;
        }
        if (word.SqlEquals("PROCEDURE") || word.SqlEquals("FOREIGN"))
        {
            rest = rest1;
            return true;
        }
        Console.WriteLine($"Expected \"PROCEDURE\" or \"FUNCTION\", got \"{word}\" instead");
        return false;
    }

    /// <summary>
    /// &lt;identifier&gt; &lt;lparen&gt; ( &lt;procedure parameter&gt; ( &lt;comma&gt; &lt;procedure parameter&gt; )* )? &lt;rparen&gt;
    /// </summary>
    private static bool ValidateProcedureHead(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!IdentifierValidator.Validate(input, out ROStr rest1))
        {
            Helper.GetNextWord(input, out ROStr word, out _);
            Console.WriteLine($"Expected an identifier. \"{word}\" is not a valid identifier");
            return false;
        }
        if (!Helper.HasNextSpecialChar(rest1, out rest1, '('))
        {
            Console.WriteLine("Expected '('.");
            return false;
        }
        if (!ValidateProcedureParameterList(rest1, out rest1))
        {
            return false;
        }
        if (!Helper.HasNextSpecialChar(rest1, out rest1, ')'))
        {
            Console.WriteLine("Expected ')'.");
            return false;
        }
        rest = rest1;
        return true;
    }

    /// <summary>
    /// ( &lt;procedure parameter&gt; ( &lt;comma&gt; &lt;procedure parameter&gt; )* )?
    /// </summary>
    private static bool ValidateProcedureParameterList(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!ProcedureParameterValidator.Validate(input, out ROStr rest1))
        {
            return true;
        }
        while (Helper.HasNextSpecialChar(rest1, out rest1, ','))
        {
            if (!ProcedureParameterValidator.Validate(input, out rest1))
            {
                Console.WriteLine("Expected another procedure parameter");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// (
    ///   RETURNS ( &lt;options clause&gt; )? (
    ///     (
    ///       (TABLE)? &lt;lparen&gt; &lt;procedure result column&gt; ( &lt;comma&gt; &lt;procedure result column&gt; )* &lt;rparen&lt;
    ///     ) | &lt;data type&gt;
    ///   )
    /// )?
    /// </summary>
    private static bool ValidateReturnStatement(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!Helper.HasNextSqlWord(input, out ROStr rest1, "RETURNS"))
        {
            return true;
        }
        OptionsClauseValidator.Validate(rest1, out rest1);
        if (!ValidateReturnValue(rest1, out rest1))
        {
            return false;
        }
        rest = rest1;
        return true;
    }

    /// <summary>
    /// (
    ///   (
    ///     (TABLE)? &lt;lparen&gt; &lt;procedure result column&gt; ( &lt;comma&gt; &lt;procedure result column&gt; )* &lt;rparen&lt;
    ///   ) | &lt;data type&gt;
    /// )
    /// </summary>
    private static bool ValidateReturnValue(ROStr input, out ROStr rest)
    {
        rest = input;
        if (DataTypeValidator.Validate(input, out ROStr rest1))
        {
            return true;
        }
        Helper.HasNextSqlWord(rest1, out rest1, "TABLE");
        if (!Helper.HasNextSpecialChar(rest1, out rest1, '('))
        {
            Console.WriteLine("Expected '('.");
            return false;
        }
        if (!ValidateReturnProcedureResultColumns(rest1, out rest1))
        {
            return false;
        }
        if (!Helper.HasNextSpecialChar(rest1, out rest1, ')'))
        {
            Console.WriteLine("Expected ')'.");
            return false;
        }
        rest = rest1;
        return true;
    }

    /// <summary>
    /// &lt;procedure result column&gt; ( &lt;comma&gt; &lt;procedure result column&gt; )*
    /// </summary>
    private static bool ValidateReturnProcedureResultColumns(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!ProcedureResultColumnValidator.Validate(input, out ROStr rest1))
        {
            Console.WriteLine("Expected at least 1 procedure result column");
            return false;
        }
        while (Helper.HasNextSpecialChar(rest1, out rest1, ','))
        {
            if (!ProcedureResultColumnValidator.Validate(input, out rest1))
            {
                Console.WriteLine("Expected another procedure result column");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ( AS <statement> )?
    /// </summary>
    private static bool ValidateAsStatement(ROStr input, out ROStr rest)
    {
        rest = input;
        if (!Helper.HasNextSqlWord(input, out ROStr rest1, "AS"))
        {
            return true;
        }
        if (!StatementValidator.Validate(rest1, out rest1))
        {
            Console.WriteLine("Expected a valid statement (loop, while, compound, if).");
            return false;
        }
        rest = rest1;
        return true;
    }

    private static bool ValidateEndOfCommand(ROStr input)
    {
        if (string.IsNullOrWhiteSpace(input.ToString()))
        {
            return true;
        }
        else
        {
            return Helper.HasNextSpecialChar(input, out ROStr rest, ';') && string.IsNullOrWhiteSpace(rest.ToString());
        }
    }
}
