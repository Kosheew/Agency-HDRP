using Characters;
using Characters.Enemy;
using System.Collections;
using Characters.Character_Interfaces;
using UnityEngine;

namespace Enemy.State
{
    public class ChaseEnemyState : BaseEnemyState
    {
        private ITargetHandler targetHandler;
        private Transform _currentTarget;
        
        public override void EnterState(EnemyContext enemy)
        {
            targetHandler = CheckTarget(enemy);
            
            enemy.CharacterAnimator.Chasing(true);
            _currentTarget = targetHandler.TargetPosition;
            
            enemy.AgentController.MoveTo(_currentTarget);
        }

        public override void UpdateState(EnemyContext enemy)
        {
            Debug.Log("Chace State");
            enemy.FootstepHandler.PlayFootstepRunSound();
            
            ChangeSpeed(enemy, enemy.EnemySetting.SprintSpeed);  

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
            
            enemy.AgentController.UpdateHandle();
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
            
        }

        public override void ExitState(EnemyContext enemy)
        {
            
        }
        
    }
}

