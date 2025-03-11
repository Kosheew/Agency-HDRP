using Characters;
using Characters.Enemy;
using UnityEngine;

namespace Enemy.State
{
    public class DeathEnemyState: IEnemyState
    {
        public void EnterState(IEnemy enemy)
        {
           //  Debug.Log("Enemy Death State Enter");
            enemy.CharacterAnimator.Death();
            enemy.Agent.speed = 0;
            enemy.Agent.isStopped = true;
        }

        public void UpdateState(IEnemy enemy)
        {
            
        }

        public void ExitState(IEnemy enemy)
        {
           
        }
    }
}