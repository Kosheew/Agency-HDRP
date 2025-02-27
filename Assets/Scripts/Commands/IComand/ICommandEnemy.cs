using Characters.Enemy;

public interface ICommandEnemy: ICommand
{
    public IEnemy Enemy { get; set; }
}
