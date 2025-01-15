
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class SelectClauseValidator
{
    private static readonly string[] AllOrDistinct = ["ALL", "DISTINCT"];

    public static bool Validate(ReadOnlySpan<char> command, out ReadOnlySpan<char> remainder)
    {
        if (!command.HasNextToken("SELECT", out remainder)) return false;

        //Remove "ALL" or "DISTINCT" from remainder if present
        remainder.HasAnyNextToken(AllOrDistinct, out remainder);
        if (remainder.IsWhiteSpace()) return false;

        // If "*" is used, the remainder is no longer relevant to this Validator
        // => http://teiid.github.io/teiid-documents/9.0.x/content/reference/BNF_for_SQL_Grammar.html#select
        if (remainder.HasNextToken("*", out remainder)) return true;

        //TODO:    
        if (SelectSublistValidator.Validate(remainder, out remainder)) return true;

        throw new NotImplementedException();
    }
}
