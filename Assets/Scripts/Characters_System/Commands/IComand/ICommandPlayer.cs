using Characters.Player;

public interface ICommandPlayer: ICommand
{ 
    public PlayerContext Player { get; set; }
}
