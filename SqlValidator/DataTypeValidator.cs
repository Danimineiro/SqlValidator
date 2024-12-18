namespace SqlValidator;
public class DataTypeValidator
{
    private static readonly Dictionary<SQLTypeName, SQLTypeModifier> Types = new() {
        { SQLTypeName.STRING, SQLTypeModifier.LENGTH },
        { SQLTypeName.VARCHAR, SQLTypeModifier.LENGTH },
        { SQLTypeName.BOOLEAN, SQLTypeModifier.NONE },
        { SQLTypeName.BYTE, SQLTypeModifier.NONE },
        { SQLTypeName.TINYINT, SQLTypeModifier.NONE },
        { SQLTypeName.SHORT, SQLTypeModifier.NONE },
        { SQLTypeName.SMALLINT, SQLTypeModifier.NONE },
        { SQLTypeName.CHAR, SQLTypeModifier.LENGTH },
        { SQLTypeName.INTEGER, SQLTypeModifier.NONE },
        { SQLTypeName.LONG, SQLTypeModifier.NONE },
        { SQLTypeName.BIGINT, SQLTypeModifier.NONE },
        { SQLTypeName.BIGINTEGER,SQLTypeModifier.LENGTH },
        { SQLTypeName.FLOAT, SQLTypeModifier.NONE },
        { SQLTypeName.REAL, SQLTypeModifier.NONE },
        { SQLTypeName.DOUBLE, SQLTypeModifier.NONE },
        { SQLTypeName.BIGDECIMAL,SQLTypeModifier.PRECISION_AND_LENGTH },
        { SQLTypeName.DECIMAL, SQLTypeModifier.PRECISION_AND_LENGTH },
        { SQLTypeName.DATE, SQLTypeModifier.NONE },
        { SQLTypeName.TIME, SQLTypeModifier.NONE },
        { SQLTypeName.TIMESTAMP, SQLTypeModifier.NONE },
        { SQLTypeName.OBJECT, SQLTypeModifier.LENGTH },
        { SQLTypeName.BLOB, SQLTypeModifier.LENGTH },
        { SQLTypeName.CLOB, SQLTypeModifier.LENGTH },
        { SQLTypeName.VARBINARY, SQLTypeModifier.LENGTH },
        { SQLTypeName.GEOMETRY, SQLTypeModifier.NONE },
        { SQLTypeName.XML, SQLTypeModifier.NONE }
    };

    public static bool Validate(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (!ValidateSimpleDataType(command, out ReadOnlySpan<char> afterSimpleDataType, out SQLTypeName typeName))
        {
            remaining = command;
            return false;
        }

        SQLTypeModifier modifier = Types[typeName];
        if (!ValidateTypeModifier(afterSimpleDataType, modifier, out ReadOnlySpan<char> afterTypeModifier))
        {
            remaining = command;
            return false;
        }

        if (!ValidateArray(afterTypeModifier, out ReadOnlySpan<char> afterArray))
        {
            remaining = command;
            return false;
        }

        remaining = afterArray;
        return true;
    }

    private static bool ValidateSimpleDataType(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining, out SQLTypeName typeName)
    {
        ReadOnlySpan<char> temp = command.TrimStart();
        Span<Range> ranges = new Range[2];
        command.SplitAny(ranges, " [(", StringSplitOptions.RemoveEmptyEntries);
        temp = command[ranges[0]];
        if (!Enum.TryParse(temp, true, out SQLTypeName sqlTypeName))
        {
            typeName = (SQLTypeName)(-1);
            remaining = command;
            return false;
        }
        typeName = sqlTypeName;
        remaining = command[ranges[1]].TrimStart();
        return true;
    }

    private static bool ValidateTypeModifier(ReadOnlySpan<char> command, SQLTypeModifier modifier, out ReadOnlySpan<char> remaining)
    {
        return modifier switch
        {
            SQLTypeModifier.NONE => ValidateNoTypeModifier(command, out remaining),
            SQLTypeModifier.LENGTH => ValidateLengthTypeModifier(command, out remaining),
            SQLTypeModifier.PRECISION_AND_LENGTH => ValidatePrecisionAndLengthModifier(command, out remaining),
            _ => throw new Exception($"Invalid enum value of ({nameof(SQLTypeModifier)}){(int)modifier}")
        };
    }

    private static bool ValidateNoTypeModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        ReadOnlySpan<char> temp = command.TrimStart();
        if (temp.IsEmpty || temp.StartsWith('('))
        {
            remaining = command;
            return false;
        }
        remaining = command;
        return true;
    }

    private static bool ValidateLengthTypeModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        ReadOnlySpan<char> temp = command.TrimStart();
        if (temp.IsEmpty)
        {
            remaining = command;
            return false;
        }
        if (temp.StartsWith('('))
        {
            temp = temp[1..].TrimStart();
            if (!uint.TryParse(temp, out uint typeLength))
            {
                remaining = command;
                return false;
            }
            temp = temp[typeLength]
        }
    }

    private static bool ValidatePrecisionAndLengthModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {

    }

    private static bool ValidateArray(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {

    }
}

internal enum SQLTypeName
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

internal enum SQLTypeModifier
{
    NONE,
    LENGTH,
    PRECISION_AND_LENGTH
}
