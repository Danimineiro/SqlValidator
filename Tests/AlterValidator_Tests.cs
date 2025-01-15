using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlValidator.DDLStatements.ALTERStatements;

namespace Tests;
public class AlterValidator_Tests
{
    [Theory]
    [InlineData(true, "FOREIGN TABLE foo OPTIONS (ADD cardinality 100)")]
    [InlineData(true, "VIRTUAL PROCEDURE bussi OPTIONS (ADD schorschigkeit true, DROP bloat)")]
    [InlineData(true, "TABLE schorschi OPTIONS (DROP cringe)")]
    [InlineData(true, "VIEW foo OPTIONS (ADD sicht false)")]
    [InlineData(false, "VIRTUAL bussi OPTIONS (ADD schorschigkeit true)")]
    [InlineData(false, "TABLE bussi (ADD schorschigkeit true)")]
    [InlineData(false, "TABLE schorsch ALTER COLUMN (DROP cringe)")]
    [InlineData(true, "TABLE schorsch ALTER COLUMN basedness OPTIONS (DROP cringe, ADD based true)")]
    [InlineData(false, "TABLE schorsch ALTER COLUMN basedness OPTIONS (DROP cringe, ADD based true")]
    public void Test_SQLAlterStatements(bool expected, string input)
    {
        bool actual = AlterValidator.Validate(input);
        Assert.Equal(expected, actual);
    }
}
