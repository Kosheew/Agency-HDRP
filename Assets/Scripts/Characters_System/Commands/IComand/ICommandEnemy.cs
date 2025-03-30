using Characters.Enemy;

public interface ICommandEnemy: ICommand
{
    public EnemyContext Enemy { get; set; }
}
