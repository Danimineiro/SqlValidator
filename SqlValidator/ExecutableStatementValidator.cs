namespace SqlValidator;

public class ExecutableStatementValidator(string command)
{
    private string Command { get; set; } = command;

    public bool Validate()
    {
        return Command.StartsWith("Select");
    }
}
