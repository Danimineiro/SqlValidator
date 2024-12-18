using SqlValidator.DDLStatements;

namespace Tests;
public class CreateTableValidator_Tests
{

    [Theory]
    [InlineData(true, "CREATE FOREIGN TABLE myForeignTable (id INT, name VARCHAR(100));")]
    [InlineData(true, "CREATE VIRTUAL VIEW myVirtualView AS SELECT id, name FROM myTable;")]
    [InlineData(true, "CREATE VIEW myView AS SELECT id, name FROM myTable;")]
    [InlineData(true, "CREATE GLOBAL TEMPORARY TABLE myTempTable (id INT, name VARCHAR(100));")]
    [InlineData(true, "CREATE FOREIGN TABLE myForeignTableWithOptions (id INT, name VARCHAR(100)) OPTIONS (option1=value1, option2=value2);")]


    public void Validate_ValidInput_ReturnsTrue(bool expected, string input)
    {

        bool result = CreateTableValidator.Validate(input.AsSpan());
        Assert.Equal(expected, result);

    }


    [Theory]
    [InlineData(false, "CREATE TABLE myForeignTable (id INT, name VARCHAR(100));")]
    [InlineData(false, "CREATE VIRTUAL myVirtualView AS SELECT id, name FROM myTable;")]
    [InlineData(false, "CREATE  myView AS SELECT id, name FROM myTable;")]
    [InlineData(false, "CREATE TEMPORARY TABLE myTempTable (id INT, name VARCHAR(100));")]
    [InlineData(false, "FOREIGN TABLE myForeignTable (id INT, name VARCHAR(100));")]

    [InlineData(false, "CREATE GLOBAL TABLE myTempTable (id INT, name VARCHAR(100));")]

    public void Validate_MissingKeyword_ReturnsFalse(bool expected, string input)
    {

        bool result = CreateTableValidator.Validate(input.AsSpan());
        Assert.Equal(expected, result);
    }

}

