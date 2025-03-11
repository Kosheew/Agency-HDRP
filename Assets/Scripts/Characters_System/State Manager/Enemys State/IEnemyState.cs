using Characters.Enemy;

namespace Characters
{
    public interface IEnemyState
    {
        void EnterState(IEnemy enemy);
        void UpdateState(IEnemy enemy);
        void ExitState(IEnemy enemy);
    }
}
