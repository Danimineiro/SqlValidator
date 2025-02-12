using SqlValidator.DDLStatements;

namespace Tests;
public class CreateTriggerValidatorTests
{

    [Fact]
    public void Validate_ValidInput_ReturnsTrue()
    {
        string input = "CREATE TRIGGER ON vw_product_sales INSTEAD OF UPDATE AS FOR EACH ROW BEGIN ATOMIC UPDATE product_sales SET quantity = NEW.quantity,price = NEW.price WHERE sale_id = OLD.sale_id;END;";


        bool result = CreateTriggerValidator.Validate(input.AsSpan());
        Assert.True(result);

        Console.WriteLine($"Input: {input}");
        Console.WriteLine($"Validation Result: {result}");

        Assert.True(result, "Expected the validation to return true for valid input.");
    }

    [Fact]
    public void Validate_MissingCreateKeyword_ReturnsFalse()
    {
        string input = "TRIGGER ON vw_employee_summary INSTEAD OF INSERT AS FOR EACH ROW BEGIN ATOMIC INSERT INTO employees (id, name, department, salary) VALUES (NEW.id, NEW.name, NEW.department, NEW.salary);END;";


        bool result = CreateTriggerValidator.Validate(input.AsSpan());
        Assert.False(result);
    }


    [Fact]
    public void Validate_MissingTriggerKeyword_ReturnsFalse()
    {
        string input = "CREATE ON vw_employee_summary INSTEAD OF INSERT AS FOR EACH ROW BEGIN ATOMIC INSERT INTO employees (id, name, department, salary) VALUES (NEW.id, NEW.name, NEW.department, NEW.salary);END;";

        bool result = CreateTriggerValidator.Validate(input.AsSpan());

        Assert.False(result);
    }

}

