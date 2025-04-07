using Characters;
using UnityEngine;
using UnityEngine.AI;
using Characters.Enemy;
using CharacterSettings;
using Commands;

namespace Enemy.State
{
    public class PatrolEnemyState : BaseEnemyState
    {
        private int _currentIndex;
     
        private float _checkInterval = 0.2f;
        private float _timeSinceLastCheck;
        private bool _isRotating = false;
        
        public override void EnterState(EnemyContext enemy)
        {
            _currentIndex = 0;

            enemy.CharacterAnimator.Chasing(false);

            enemy.AgentController.StartPatrol();
        }

        public override void UpdateState(EnemyContext enemy)
        { 
            enemy.FootstepHandler.PlayFootstepWalkSound();
            
            ChangeSpeed(enemy, enemy.EnemySetting.MoveSpeed);  
            
            if (CheckTarget(enemy) != null)
            {
                enemy.CommandEnemy.CreateChasingCommand(enemy);
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
