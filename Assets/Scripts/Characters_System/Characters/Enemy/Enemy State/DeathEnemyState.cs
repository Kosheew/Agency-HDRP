using Characters;
using Characters.Enemy;
using UnityEngine;

namespace Enemy.State
{
    public class DeathEnemyState: IEnemyState
    {
        public void EnterState(EnemyContext enemy)
        {
           //  Debug.Log("Enemy Death State Enter");
            enemy.CharacterAnimator.Death();
            enemy.Agent.speed = 0;
            enemy.Agent.isStopped = true;
        }

        public void UpdateState(EnemyContext enemy)
        {
            
        }

        public void ExitState(EnemyContext enemy)
        {
           
        }
    }
}