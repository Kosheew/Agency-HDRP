using Characters.Enemy;

namespace Characters
{
    public interface IEnemyState
    {
        void EnterState(EnemyContext enemy);
        void UpdateState(EnemyContext enemy);
        void ExitState(EnemyContext enemy);
    }
}
