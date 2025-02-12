using SqlValidator;

namespace Tests;
public class DataTypeValidator_Tests
{
    [Theory]
    [InlineData(true, "boolean", "", SQLTypeName.BOOLEAN)]
    [InlineData(true, "DoUbLe", "", SQLTypeName.DOUBLE)]
    [InlineData(true, "    DATE", "", SQLTypeName.DATE)]
    [InlineData(true, " FLOAT  jsjs", "  jsjs", SQLTypeName.FLOAT)]
    [InlineData(true, " varchar(20)  ", "(20)  ", SQLTypeName.VARCHAR)]
    [InlineData(false, " charlos", " charlos", (SQLTypeName)(-1))]
    public void Test_Simple(bool expected, string input, string expectedRest, SQLTypeName expectedTypeName)
    {
        bool actual = DataTypeValidator.ValidateSimpleDataType(input, out ReadOnlySpan<char> rest, out SQLTypeName typeName);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
        Assert.Equal(expectedTypeName, typeName);
    }

    [Theory]
    [InlineData(true, "(12)", "")]
    [InlineData(false, "(-6)", "(-6)")]
    [InlineData(true, "  (  16  )  shahld", "  shahld")]
    [InlineData(true, "", "")]
    [InlineData(true, "sghwj", "sghwj")]
    [InlineData(false, "(5, 2)", "(5, 2)")]
    public void Test_Length(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.ValidateLengthTypeModifier(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "(12)", "")]
    [InlineData(false, "(-6)", "(-6)")]
    [InlineData(true, "  (  16  )  shahld", "  shahld")]
    [InlineData(true, "", "")]
    [InlineData(true, "sghwj", "sghwj")]
    [InlineData(true, "(5, 2)", "")]
    public void Test_PrecisionAndLength(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.ValidatePrecisionAndLengthModifier(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "[]", "")]
    [InlineData(true, "[][]", "")]
    [InlineData(true, "[][][]", "")]
    [InlineData(true, " [] [   ]   ed", "   ed")]
    [InlineData(true, "ed", "ed")]
    [InlineData(true, "[", "[")]
    [InlineData(true, "]", "]")]
    public void Test_Array(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.ValidateArray(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(false, "TIME(3)", "TIME(3)")]
    [InlineData(true, "Char", "")]
    [InlineData(true, "Char(10)", "")]
    [InlineData(false, "Char(4, 5)", "Char(4, 5)")]
    public void Test_WithLength(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.Validate(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "DECIMAL", "")]
    [InlineData(true, "DECIMAL(40)", "")]
    [InlineData(true, "DECIMAL(20,30)", "")]
    [InlineData(false, "DECIMAL(3, 4, 5 ), ", "DECIMAL(3, 4, 5 ), ")]
    [InlineData(true, "   DECIMAL   (  62    ,      5    )   SQL is overrated and you won't convince me otherwise.", "   SQL is overrated and you won't convince me otherwise.")]
    [InlineData(true, "       BIGDECIMAL    (  43    ,     14     )         Hello", "         Hello")]
    public void Test_WithPrecisionAndLength(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.Validate(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "INTEGER[]", "")]
    [InlineData(true, "INTEGER[][]", "")]
    [InlineData(true, "INTEGER[][][]", "")]
    [InlineData(true, "INTEGER[][][][][][][][][][][][][][][][][][][][]", "")]
    [InlineData(true, "   INTEGER  [  ]     [ ]  [        ]", "")]
    public void Test_WithArray(bool expected, string input, string expectedRest)
    {
        bool actual = DataTypeValidator.Validate(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "CHAR(10)[]", "")]
    [InlineData(true, "DECIMAL(10, 10)[]", "")]
    [InlineData(true, " BIGDECIMAL  ( 26  ,  27)   [    ] [   ]", "")]
    public void Test_WithPrecisionLengthAndArray(bool expected, string input, string expectedRest)
    {

        bool actual = DataTypeValidator.Validate(input, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedRest, rest);
    }
}
