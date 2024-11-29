using SqlValidator;

namespace SqlValidatorConsole;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a sql statement to test:");
        string? command;
        while (true)
        {
            if (Console.ReadLine() is not string input)
            {
                Console.WriteLine("Something went wrong, please try again:");
                continue;
            }

            command = input;
            break;
        }

        ExecutableStatementValidator validator = new(command);

        string result = validator.Validate() ? string.Empty : "not ";

        Console.WriteLine($"Your input was {result}a succesful input.");
    }
}
