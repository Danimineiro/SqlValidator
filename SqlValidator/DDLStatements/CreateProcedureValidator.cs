using SqlValidator.Identifiers;

namespace SqlValidator.DDLStatements;
public class CreateProcedureValidator
{
    public static bool Validate(ReadOnlySpan<char> command)
    {
        if (!Helper.GetNextWord(command, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest) || !word.SqlEquals("create"))
        {
            return false;
        }
        if (!Helper.GetNextWord(rest, out word, out rest))
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
        // <identifier>
        if (!QuotedIdValidator.Validate(rest, out rest))
        {
            return false;
        }
        // <lparen>
        if (!Helper.GetNextWord(rest, out word, out rest) || word.Length != 1 || word[0] != '(')
        {
            return false;
        }
        /*
        // ( <procedure parameter> ( <comma> <procedure parameter> )* )?
        // <procedure parameter>
        if (ValidateProcedureParameter(remaining, out remaining))
        {
            // ( <comma> <procedure parameter> )*
            while (true)
            {
                // <comma>
                if (!remaining.StartsWith(','))
                {
                    break;
                }
                // <procedure parameter>
                if (!ValidateProcedureParameter(remaining, out remaining))
                {
                    return false;
                }
            }
        }
        // <rparen>
        if (!remaining.StartsWith(')'))
        {
            return false;
        }
        remaining = remaining[1..];
        // ( RETURNS ( <options clause> )? ( ( ( TABLE )? <lparen> <procedure result column> ( <comma> <procedure result column> )* <rparen> ) | <data type> ) )?
        // RETURNS
        if (remaining.StartsWith("RETURNS"))
        {
            remaining = remaining[7..];
            // ( <options clause> )?
            int previousLength = remaining.Length;
            bool result = ValidateOptionsClause(remaining, out remaining);
            if (remaining.Length < previousLength && !result)
            {
                return false;
            }

            // ( ( ( TABLE )? <lparen> <procedure result column> ( <comma> <procedure result column>)* <rparen> ) | <data type> )
            // <data type>
            if (ValidateDataType(remaining, out remaining))
            {

            }
            // ( ( TABLE )? <lparen> <procedure result column> ( <comma> <procedure result column>)* <rparen> )
            else
            {
                remaining.SqlStartsWith("TABLE", out remaining);
                if (!remaining.StartsWith('('))
                {
                    return false;
                }
                remaining = remaining[1..];

            }
        }
        /*
         *      (
         *          RETURNS
         *          ...
         *          (
         *              (
         *                  (
         *                      TABLE
         *                  )?
         *                  <lparen>
         *                  <procedure result column>
         *                  (
         *                      <comma>
         *                      <procedure result column>
         *                  )*
         *                  <rparen>
         *              )
         *              | <data type>
         *          )
         *      )?
         *      (
         *          <options clause>
         *      )?
         *      (
         *          AS <statement>
         *      )?
         *  )
         */

        return true;
    }

    private static bool ValidateProcedureParameter(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        throw new NotImplementedException();
    }

    private static bool ValidateOptionsClause(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        throw new NotImplementedException();
    }

    private static bool ValidateDataType(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        throw new NotImplementedException();

        // validate types without array length indicator
        string[] typesWithoutLength = [
            "BOOLEAN",
            "BYTE", "TINYINT", "SHORT", "SMALLINT", "INTEGER", "LONG", "BIGINT",
            "FLOAT", "REAL", "DOUBLE",
            "DATE", "TIME", "TIMESTAMP",
            "GEOMETRY", "XML"
        ];
        string[] typesWithLength = [
            "STRING", "VARCHAR", "CHAR",
            "BIGINTEGER",
            "OBJECT", "BLOB", "CLOB", "VARBINARY"
        ];
        string[] typesWithComplicatedLength = [
            "BIGDECIMAL", "DECIMAL"
        ];
        if (!command.SqlStartsWithAny(typesWithoutLength, out remaining))
        {
            return false;
        }
    }

    private static bool ValidateProcedureResultColumn(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        remaining = command;
        throw new NotImplementedException();
        if (!IdentifierValidator.Validate(command, out remaining))
        {
            return false;
        }
        if (!ValidateDataType(command, out remaining))
        {
            return false;
        }

    }
}
