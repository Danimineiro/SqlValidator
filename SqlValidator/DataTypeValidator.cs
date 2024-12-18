namespace SqlValidator;
internal class DataTypeValidator
{
    private static readonly Dictionary<SQLTypeNames, SQLTypeLengthMode> Types = new() {
        { SQLTypeNames.STRING, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.VARCHAR, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.BOOLEAN, SQLTypeLengthMode.NONE },
        { SQLTypeNames.BYTE, SQLTypeLengthMode.NONE },
        { SQLTypeNames.TINYINT, SQLTypeLengthMode.NONE },
        { SQLTypeNames.SHORT, SQLTypeLengthMode.NONE },
        { SQLTypeNames.SMALLINT, SQLTypeLengthMode.NONE },
        { SQLTypeNames.CHAR, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.INTEGER, SQLTypeLengthMode.NONE },
        { SQLTypeNames.LONG, SQLTypeLengthMode.NONE },
        { SQLTypeNames.BIGINT, SQLTypeLengthMode.NONE },
        { SQLTypeNames.BIGINTEGER,SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.FLOAT, SQLTypeLengthMode.NONE },
        { SQLTypeNames.REAL, SQLTypeLengthMode.NONE },
        { SQLTypeNames.DOUBLE, SQLTypeLengthMode.NONE },
        { SQLTypeNames.BIGDECIMAL,SQLTypeLengthMode.DOUBLE },
        { SQLTypeNames.DECIMAL, SQLTypeLengthMode.DOUBLE },
        { SQLTypeNames.DATE, SQLTypeLengthMode.NONE },
        { SQLTypeNames.TIME, SQLTypeLengthMode.NONE },
        { SQLTypeNames.TIMESTAMP, SQLTypeLengthMode.NONE },
        { SQLTypeNames.OBJECT, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.BLOB, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.CLOB, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.VARBINARY, SQLTypeLengthMode.SIMPLE },
        { SQLTypeNames.GEOMETRY, SQLTypeLengthMode.NONE },
        { SQLTypeNames.XML, SQLTypeLengthMode.NONE }
    };

    public static bool Validate(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        remaining = command;
        Span<Range> ranges = new Range[2];
        command.SplitAny(ranges, " [(", StringSplitOptions.RemoveEmptyEntries);
        ReadOnlySpan<char> temp = command[ranges[0]];
        if (!Enum.TryParse(temp, true, out SQLTypeNames sqlTypeName))
        {
            return false;
        }
        SQLTypeLengthMode typeLengthMode = Types[sqlTypeName];
        temp = command[ranges[1]].TrimStart();
        if (temp.Length == 0)
        {
            remaining = string.Empty;
            return true;
        }
        switch (typeLengthMode)
        {
            case SQLTypeLengthMode.NONE:
                if (temp[0] == '(')
                {
                    return false;
                }
                break;
            case SQLTypeLengthMode.SIMPLE:
                break;
            case SQLTypeLengthMode.DOUBLE:
                break;
            default:
                throw new Exception($"Invalid enum value of (SQLTypeLengthMode){(int)typeLengthMode}");
        }

        throw new NotImplementedException();
    }
}

internal enum SQLTypeNames
{
    STRING,
    VARCHAR,
    BOOLEAN,
    BYTE,
    TINYINT,
    SHORT,
    SMALLINT,
    CHAR,
    INTEGER,
    LONG,
    BIGINT,
    BIGINTEGER,
    FLOAT,
    REAL,
    DOUBLE,
    BIGDECIMAL,
    DECIMAL,
    DATE,
    TIME,
    TIMESTAMP,
    OBJECT,
    BLOB,
    CLOB,
    VARBINARY,
    GEOMETRY,
    XML
}

internal enum SQLTypeLengthMode
{
    NONE,
    SIMPLE,
    DOUBLE
}
