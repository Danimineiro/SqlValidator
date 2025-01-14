
using System.Buffers;

namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class NonReservedIdentifierValidator
{
    #region Keywords
    private static readonly string[] keywords =
    [
        "INSTEAD",
        "VIEW",
        "ENABLED",
        "DISABLED",
        "KEY",
        "SERIAL",
        "TEXTAGG",
        "COUNT",
        "ROW_NUMBER",
        "RANK",
        "DENSE_RANK",
        "SUM",
        "AVG",
        "MIN",
        "MAX",
        "EVERY",
        "STDDEV_POP",
        "STDDEV_SAMP",
        "VAR_SAMP",
        "VAR_POP",
        "DOCUMENT",
        "CONTENT",
        "TRIM",
        "EMPTY",
        "ORDINALITY",
        "PATH",
        "FIRST",
        "LAST",
        "NEXT",
        "SUBSTRING",
        "EXTRACT",
        "TO_CHARS",
        "TO_BYTES",
        "TIMESTAMPADD",
        "TIMESTAMPDIFF",
        "QUERYSTRING",
        "NAMESPACE",
        "RESULT",
        "INDEX",
        "ACCESSPATTERN",
        "AUTO_INCREMENT",
        "WELLFORMED",
        "SQL_TSI_FRAC_SECOND",
        "SQL_TSI_SECOND",
        "SQL_TSI_MINUTE",
        "SQL_TSI_HOUR",
        "SQL_TSI_DAY",
        "SQL_TSI_WEEK",
        "SQL_TSI_MONTH",
        "SQL_TSI_QUARTER",
        "SQL_TSI_YEAR",
        "TEXTTABLE",
        "ARRAYTABLE",
        "SELECTOR",
        "SKIP",
        "WIDTH",
        "PASSING",
        "NAME",
        "ENCODING",
        "COLUMNS",
        "DELIMITER",
        "QUOTE",
        "HEADER",
        "NULLS",
        "OBJECTTABLE",
        "VERSION",
        "INCLUDING",
        "EXCLUDING",
        "XMLDECLARATION",
        "VARIADIC",
        "RAISE",
        "EXCEPTION",
        "CHAIN",
        "JSONARRAY_AGG",
        "JSONOBJECT",
        "PRESERVE"
    ];
    #endregion Keywords

    private static SearchValues<string> Keywords { get; } = SearchValues.Create(keywords, StringComparison.InvariantCultureIgnoreCase);

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;
        if (!input.TryGetNextToken(out ReadOnlySpan<char> token)) return false;

        if (Keywords.Contains(token.ToString()))
        {
            remainder = input[token.Length..].TrimStart();
            return true;
        }

        return false;
    }
}
