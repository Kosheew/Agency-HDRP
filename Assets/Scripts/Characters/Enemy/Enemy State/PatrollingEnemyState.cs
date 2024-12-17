using Characters;
using UnityEngine;
using UnityEngine.AI;
using Characters.Enemy;
using CharacterSettings;
using Commands;

namespace Enemy.State
{
    public class PatrollingEnemyState : BaseEnemyState
    {
        private int _currentIndex;
        
        public override void EnterState(IEnemy enemy)
        {
            _currentIndex = 0;
            enemy.Agent.isStopped = false;
            enemy.Agent.SetDestination(enemy.PatrolTargets[_currentIndex].position);
        }

        public override void UpdateState(IEnemy enemy)
        { 
            if (CanSeeTarget(enemy, enemy.TargetPlayer))
            {
                enemy.CommandEnemy.CreateChasingCommand(enemy);
                return;
            }
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
            enemy.FootstepHandler.PlayFootstepSound();
            
            if (enemy.Agent.remainingDistance < 0.1f)
            {
                _currentIndex = (_currentIndex + 1) % enemy.PatrolTargets.Length;
                enemy.Agent.SetDestination(enemy.PatrolTargets[_currentIndex].position);
            }
        }

        public override void ExitState(IEnemy enemy)
        {
            enemy.Agent.isStopped = true;
        }
    }
}
