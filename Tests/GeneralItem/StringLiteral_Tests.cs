using SqlValidator.DDLStatements.GeneralItemValidators;

namespace GeneralItem;
public class StringLiteral_Tests
{
    [Theory]
    [InlineData(true, "'Hello, World!'", "")]
    [InlineData(false, "'Hello, World!", "'Hello, World!"),]
    [InlineData(true, "'It''s-a me, Mario!'", "")]
    [InlineData(true, "'It''s-a me,' Mario!", " Mario!")]
    [InlineData(true, "   ' Heehee '  ", "  ")]
    public void Test(bool expected, string input, string remaining)
    {
        bool actual = StringLiteralValidator.Validate(input, out var actualRest);
        Assert.Equal(expected, actual);
        Assert.Equal(remaining, actualRest);
    }
}
