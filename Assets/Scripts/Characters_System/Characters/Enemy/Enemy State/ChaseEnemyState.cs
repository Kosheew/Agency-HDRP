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
        
        public override void EnterState(EnemyContext enemy)
        {
            
            targetHandler = CheckTarget(enemy);
            
            enemy.CharacterAnimator.Chasing(true);
            enemy.TargetTransform = targetHandler.TargetPosition;
            
            enemy.AgentController.MoveTo(enemy.TargetTransform);
            Debug.Log("Enter Chase State");
        }

        public override void UpdateState(EnemyContext enemy)
        {
            enemy.FootstepHandler.PlayFootstepRunSound();
            
            ChangeSpeed(enemy, enemy.EnemySetting.SprintSpeed);  

            if (CheckTarget(enemy) == null)
            {
                enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                return;
            }
            
            float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.TargetTransform.position);
            bool canSeePlayer = enemy.AgentController.CheckPlayerVisibility();
    
            if (distanceToTarget <= enemy.EnemySetting.AttackDistance )
            {
                enemy.CommandEnemy.CreateAttackCommand(enemy);
                return;
            }
            
            enemy.AgentController.UpdateChase();
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
            
        }

        public override void ExitState(EnemyContext enemy)
        {
            enemy.AgentController.Resume();
        }
        
    }
}

