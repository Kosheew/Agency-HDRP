using Characters;

public interface ICommandPlayer: ICommand
{ 
    public IPlayer Player { get; set; }
}
