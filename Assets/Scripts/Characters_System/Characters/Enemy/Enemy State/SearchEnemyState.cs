using Characters;
using Characters.Enemy;
using UnityEngine;

namespace Enemy.State
{
    public class SearchEnemyState: BaseEnemyState
    {
        private float _decayDelay = 2f;
        private float _currentDecayTime;
        
        public override void EnterState(EnemyContext enemy)
        {
            Debug.Log("Enter Search");
            _currentDecayTime = _decayDelay;
        }

        public override void UpdateState(EnemyContext enemy)
        {
            if (enemy.TargetTransform== null)
            {
                enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                return;
            }
            
            enemy.FootstepHandler.PlayFootstepWalkSound();
            ChangeSpeed(enemy, enemy.EnemySetting.MoveSpeed);  
            
            float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.TargetTransform.position);
            
            if (distanceToTarget > 3f)
            {
                enemy.Agent.SetDestination(enemy.TargetTransform.position);

            }
            else
            {
                ChangeSpeed(enemy, 0, 5);  
                SlowDownBeforeStopping(enemy);
                _currentDecayTime -= Time.deltaTime;
                 
                 if (_currentDecayTime <= 0)
                 {
                     enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                 }
            }
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
        }

        public override void ExitState(EnemyContext enemy)
        {
            Debug.Log("Exit Search");
            enemy.Agent.isStopped = false;
        }
    }
}