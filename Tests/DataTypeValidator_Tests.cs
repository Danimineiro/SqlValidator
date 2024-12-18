using SqlValidator;

namespace Tests;
public class DataTypeValidator_Tests
{
    private static void Test(bool expectedResult, ReadOnlySpan<char> input, ReadOnlySpan<char> expectedRemaining)
    {
        bool actualResult = DataTypeValidator.Validate(input, out ReadOnlySpan<char> actualRemaining);
        if (expectedResult == actualResult)
        {
            if (actualResult) // remaining only matters if result is true
            {
                Assert.Equal(expectedRemaining, actualRemaining); 
            }
        }
        else
        {
            Assert.Fail($"Unexpected result: '{actualResult}' for \"{input}\"");
        }
    }

    [Fact]
    public void Test_Lowercase() => Test(true, "boolean", string.Empty);

    [Fact]
    public void Test_MixedCase() => Test(true, "DoUbLe", string.Empty);

    [Fact]
    public void Test_LeftPad() => Test(true, "    DATE", string.Empty);

    [Fact]
    public void Test_InvalidLength() => Test(false, "TIME(3)", string.Empty);

    [Fact]
    public void Test_CanHaveLengthButDoesnt() => Test(true, "Char", string.Empty);

    [Fact]
    public void Test_CanHaveLengthAndDoes() => Test(true, "Char(10)", string.Empty);

    [Fact]
    public void Test_CanHaveLengthAndHasTooMany() => Test(false, "Char(4, 5)", string.Empty);

    [Fact]
    public void Test_CanHavePrecisionAndLengthButHasNone() => Test(true, "DECIMAL", string.Empty);

    [Fact]
    public void Test_CanHavePrecisionAndLengthButOnlyHasPrecision() => Test(true, "DECIMAL(40)", string.Empty);

    [Fact]
    public void Test_CanHavePrecisionAndLengthAndHasBoth() => Test(true, "DECIMAL(20,30)", string.Empty);

    [Fact]
    public void Test_CanHavePrecisionAndLengthAndHasTooMany() => Test(false, "   DECIMAL (3, 4, 5 ), ", ", ");

    [Fact]
    public void Test_LeaveRemaining() => Test(true, "   DECIMAL   (  62    ,      5    )   SQL is overrated and you won't convince me otherwise.", "   SQL is overrated and you won't convince me otherwise.");

    [Fact]
    public void Test_ALotOfPadding() => Test(true, "       BIGDECIMAL    (  43    ,     14     )         Hello", "         Hello");

    [Fact]
    public void Test_Array() => Test(true, "INTEGER[]", string.Empty);

    [Fact]
    public void Test_2dArray() => Test(true, "INTEGER[][]", string.Empty);

    [Fact]
    public void Test_3dArray() => Test(true, "INTEGER[][][]", string.Empty);

    [Fact]
    public void Test_MultidimensionalBeingThatWeAsMereHumansCouldNeverEvenDreamToPerceive() => Test(true, "INTEGER[][][][][][][][][][][][][][][][][][][][]", string.Empty);

    [Fact]
    public void Test_ArrayWithPadding() => Test(true, "   INTEGER  [  ]     [ ]  [        ]", string.Empty);

    [Fact]
    public void Test_LengthAndArray() => Test(true, "CHAR(10)[]", string.Empty);

    [Fact]
    public void Test_PrecisionLengthAndArray() => Test(true, "DECIMAL(10, 10)[]", string.Empty);

    [Fact]
    public void Test_PrecisionLengthArrayAndPadding() => Test(true, " BIGDECIMAL  ( 26  ,  27)   [    ] [   ]", string.Empty);
}
