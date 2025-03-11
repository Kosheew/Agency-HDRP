using System.Collections.Generic;

public class CommandInvoker
{
    private List<ICommand> _commands = new List<ICommand>();

    public void SetCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public void ExecuteCommands()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
        _commands.Clear();
    }
}