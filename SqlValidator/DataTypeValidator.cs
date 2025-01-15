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
            if (modifier == SQLTypeModifier.NONE && afterTypeModifier.StartsWith('('))
            {
                remaining = command;
                return false;
            }
            remaining = command;
        }

        if (!ValidateArray(afterTypeModifier, out ReadOnlySpan<char> afterArray))
        {
            remaining = command;
        }

        remaining = afterArray;
        return true;
    }

    public static bool ValidateSimpleDataType(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining, out SQLTypeName typeName)
    {
        if (!Helper.GetNextWord(command, out ReadOnlySpan<char> sqlType, out remaining) ||
            !Enum.TryParse(sqlType, true, out typeName))
        {
            remaining = command;
            typeName = (SQLTypeName)(-1);
            return false;
        }
        return true;
    }

    public static bool ValidateTypeModifier(ReadOnlySpan<char> command, SQLTypeModifier modifier, out ReadOnlySpan<char> remaining)
    {
        return modifier switch
        {
            SQLTypeModifier.NONE => ValidateNoTypeModifier(command, out remaining),
            SQLTypeModifier.LENGTH => ValidateLengthTypeModifier(command, out remaining),
            SQLTypeModifier.PRECISION_AND_LENGTH => ValidatePrecisionAndLengthModifier(command, out remaining),
            _ => throw new Exception($"Invalid enum value of ({nameof(SQLTypeModifier)}){(int)modifier}")
        };
    }

    public static bool ValidateNoTypeModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        if (Helper.GetNextWord(command, out ReadOnlySpan<char> word, out ReadOnlySpan<char> _) ||
            word.Length == 1 && word[0] == '(')
        {
            remaining = command;
            return false;
        }
        remaining = command;
        return true;
    }

    public static bool ValidateLengthTypeModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        bool gotOpen = Helper.GetNextWord(command, out ReadOnlySpan<char> open, out ReadOnlySpan<char> afterOpen);
        if (!gotOpen || open.Length >= 1 && open[0] != '(')
        {
            remaining = command;
            return true;
        }
        if (!Helper.GetNextWord(afterOpen, out ReadOnlySpan<char> length, out ReadOnlySpan<char> afterLength) || !uint.TryParse(length, out uint _) ||
            !Helper.GetNextWord(afterLength, out ReadOnlySpan<char> close, out ReadOnlySpan<char> afterClose) || close.Length != 1 || close[0] != ')')
        {
            remaining = command;
            return false;
        }
        remaining = afterClose;
        return true;
    }

    public static bool ValidatePrecisionAndLengthModifier(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        bool gotOpen = Helper.GetNextWord(command, out ReadOnlySpan<char> open, out ReadOnlySpan<char> afterOpen);
        if (!gotOpen || open.Length >= 1 && open[0] != '(')
        {
            remaining = command;
            return true;
        }
        if (!Helper.GetNextWord(afterOpen, out ReadOnlySpan<char> num1, out ReadOnlySpan<char> afterNum1) || !uint.TryParse(num1, out uint _))
        {
            remaining = command;
            return false;
        }
        if (Helper.GetNextWord(afterNum1, out ReadOnlySpan<char> commaOrClose, out ReadOnlySpan<char> afterCommaOrClose) && commaOrClose.Length == 1)
        {
            switch (commaOrClose[0])
            {
                case ')':
                    remaining = afterCommaOrClose;
                    return true;

                case ',':
                    break;

                default:
                    remaining = command;
                    return false;
            }
        }
        if (!Helper.GetNextWord(afterCommaOrClose, out ReadOnlySpan<char> length, out ReadOnlySpan<char> afterLength) || !uint.TryParse(length, out uint _) ||
            !Helper.GetNextWord(afterLength, out ReadOnlySpan<char> close, out ReadOnlySpan<char> afterClose) || close.Length != 1 || close[0] != ')')
        {
            remaining = command;
            return false;
        }
        remaining = afterClose;
        return true;
    }

    public static bool ValidateArray(ReadOnlySpan<char> command, out ReadOnlySpan<char> remaining)
    {
        ReadOnlySpan<char> temp = command;
        while (true)
        {
            if (Helper.GetNextWord(temp, out ReadOnlySpan<char> open, out ReadOnlySpan<char> afterOpen) && open.Length == 1 && open[0] == '[')
            {
                if (Helper.GetNextWord(afterOpen, out ReadOnlySpan<char> close, out ReadOnlySpan<char> afterClose) && close.Length == 1 && close[0] == ']')
                {
                    temp = afterClose;
                }
                else
                {
                    remaining = command;
                    return true;
                }
            }
            else
            {
                remaining = temp;
                return true;
            }
        }
    }
}

public enum SQLTypeName
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

public enum SQLTypeModifier
{
    NONE,
    LENGTH,
    PRECISION_AND_LENGTH
}
