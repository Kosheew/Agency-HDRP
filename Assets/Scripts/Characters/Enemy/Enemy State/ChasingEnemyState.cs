using Characters;
using Characters.Enemy;
using System.Collections;
using UnityEngine;

namespace Enemy.State
{
    public class ChasingEnemyState : BaseEnemyState
    {
        private Transform _currentTarget;
        
        public override void EnterState(IEnemy enemy)
        {
            enemy.Agent.isStopped = false;
            _currentTarget = enemy.TargetPlayer.TransformMain;
            
        }

        public override void UpdateState(IEnemy enemy)
        {
            if (!CheckTarget(enemy, enemy.TargetPlayer))
            {
                enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                return;
            }
            
            if (IsTargetInRange(enemy, enemy.TargetPlayer, enemy.EnemySetting.AttackDistance))
            {
                enemy.CommandEnemy.CreateAttackCommand(enemy);
                return;
            }
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
          //  enemy.FootstepHandler.PlayFootstepSound();
            enemy.Agent.SetDestination(_currentTarget.position);
        }

        public override void ExitState(IEnemy enemy)
        {
            enemy.Agent.isStopped = true;
            enemy.CharacterAnimator.Running(0);
        }
        
    }
}

