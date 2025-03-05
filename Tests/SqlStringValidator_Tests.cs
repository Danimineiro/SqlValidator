using SqlValidator;

namespace Tests;
public class SqlStringValidator_Tests
{
    [Theory]
    [InlineData([true, "'Hello'", ""])]
    [InlineData([false, "'Hello", "'Hello"])]
    [InlineData([true, "'Hello' World", " World"])]
    [InlineData([false, "World 'Hello'", "World 'Hello'"])]
    [InlineData([true, "'Hello World'", ""])]
    [InlineData([true, "'''Hello''''World'''", ""])]
    [InlineData([true, "'Hello'World'", "World'"])]
    [InlineData([false, "", ""])]
    [InlineData([true, "''", ""])]
    [InlineData([true, "' '", ""])]
    public void Test_SqlStrings(bool expected, string input, string expectedRemainder)
    {
        bool result = SqlStringValidator.Validate(input, out ReadOnlySpan<char> remaining);
        Assert.Equal(expected, result);
        Assert.Equal(expectedRemainder, remaining);
    }
}
