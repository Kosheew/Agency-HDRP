using Characters;
using Characters.Enemy;
using System.Collections;
using Characters.Character_Interfaces;
using UnityEngine;

namespace Enemy.State
{
    public class ChasingEnemyState : BaseEnemyState
    {
        private ITargetHandler targetHandler;
        private Transform _currentTarget;
        
        public override void EnterState(IEnemy enemy)
        {
           // enemy.Agent.isStopped = false;
            targetHandler = CheckTarget(enemy);
            
            enemy.CharacterAnimator.Chasing(true);
            _currentTarget = targetHandler.TargetPosition;
        }

        public override void UpdateState(IEnemy enemy)
        {
            enemy.FootstepHandler.PlayFootstepRunSound();
            
            ChangeSpeed(enemy, enemy.EnemySetting.SprintSpeed);  
            // enemy.Agent.speed = Mathf.Lerp(enemy.Agent.speed, enemy.EnemySetting.SprintSpeed, Time.deltaTime * 15);
            if (CheckTarget(enemy) == null)
            {
                enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                return;
            }
            
            if (IsTargetInRange(enemy, targetHandler, enemy.EnemySetting.AttackDistance))
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
            // enemy.Agent.isStopped = true;
        }
        
    }
}

