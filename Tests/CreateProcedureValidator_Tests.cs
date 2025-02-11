using SqlValidator.DDLStatements;
namespace Tests;
public class CreateProcedureValidator_Tests
{
    [Theory]
    [InlineData(false, "dged")]
    [InlineData(false, "wgdhwld")]
    [InlineData(false, "wdaobdsaidiawod")]
    [InlineData(false, "27722")]
    [InlineData(false, "HSH")]
    [InlineData(false, "djwjd")]
    public void Test(bool expected, string input)
    {
        bool actual = CreateProcedureValidator.Validate(input);
        Assert.Equal(expected, actual);
    }
}
