using Characters;
using Characters.Enemy;
using UnityEngine;

namespace Enemy.State
{
    public class SuspiciousEnemyState: BaseEnemyState
    {
        private float _decayDelay = 2f;
        
        public override void EnterState(EnemyContext enemy)
        {

        }

        public override void UpdateState(EnemyContext enemy)
        {
            ChangeSpeed(enemy, 0, 5);  
            SlowDownBeforeStopping(enemy);
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
            _decayDelay -= Time.deltaTime;
            
            if(_decayDelay > 0) return;
            
            enemy.CommandEnemy.CreateSearchCommand(enemy);
        }

        public override void ExitState(EnemyContext enemy)
        {
            Debug.Log("Exit Suspicious State");
            enemy.Agent.isStopped = false;
        }
    }
}