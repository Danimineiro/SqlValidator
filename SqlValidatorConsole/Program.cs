using SqlValidator.DDLStatements;

namespace SqlValidatorConsole;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a sql statement to test:");
        //Console.SetIn(new StringReader(
        //    """
        //    CREATE TRIGGER ON vw_customer_orders
        //    INSTEAD OF DELETE
        //    AS FOR EACH ROW
        //    BEGIN ATOMIC
        //        DELETE FROM customer_orders
        //        WHERE order_id = OLD.order_id;
        //    END;
        //    """));

        string? command;
        while (true)
        {
            if (Console.ReadLine() is not string input)
            {
                Console.WriteLine("Something went wrong, please try again:");
                continue;
            }

            command = input;// + Console.In.ReadToEnd();
            break;
        }

        string result = DDLStatementValidator.Validate(command) ? string.Empty : "not ";

        Console.WriteLine($"Your input was {result}a succesful input.");
    }
}
