using Characters;

/// <summary>
/// Represents a command that can be executed.
/// Implementing classes should define the action to be performed when the command is executed.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    
    void Execute();
}
