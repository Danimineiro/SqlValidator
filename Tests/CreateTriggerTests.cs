using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlValidator.DDLStatements;

namespace Tests;
public class CreateTriggerTests
{
    [Theory]
    [InlineData(true, "CREATE TRIGGER ON vw INSTEAD OF INSERT AS FOR EACH ROW BEGIN ATOMIC ... END")]
    [InlineData(false, "CREATE ON vw INSTEAD OF INSERT AS FOR EACH ROW BEGIN ATOMIC ... END")]
    [InlineData(false, "TRIGGER ON vw INSTEAD OF INSERT AS FOR EACH ROW BEGIN ATOMIC ... END")]
    [InlineData(true, "CREATE TRIGGER ON vw INSTEAD OF UPDATE AS FOR EACH ROW BEGIN ATOMIC ... END")]
    [InlineData(true, "CREATE TRIGGER ON vw INSTEAD OF DELETE AS FOR EACH ROW BEGIN ATOMIC ... END")]
    [InlineData(true, "CREATE TRIGGER ON vw INSTEAD OF INSERT AS FOR EACH ROW IF (x = 5) BEGIN ATOMIC ... END")]
    public void Test_CreateTriggerStatements(bool expected, string input)
    {
        bool actual = CreateTriggerValidator.Validate(input);
        Assert.Equal(expected, actual);
    }
}
