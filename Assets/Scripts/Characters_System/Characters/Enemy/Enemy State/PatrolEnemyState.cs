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
            //enemy.Agent.isStopped = false;

           // enemy.Agent.speed = enemy.EnemySetting.MoveSpeed;

            enemy.CharacterAnimator.Chasing(false);
            
            enemy.Agent.SetDestination(enemy.PatrolTargets[_currentIndex].position);
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
            
            /*if (_isRotating)
            {
                RotateTowards(enemy, enemy.PatrolTargets[_currentIndex]);
                return; // Не починати рух, поки не завершиться поворот
            }*/
            
            if (enemy.Agent.remainingDistance < 1f)
            {
                _currentIndex = (_currentIndex + 1) % enemy.PatrolTargets.Length;
                enemy.Agent.SetDestination(enemy.PatrolTargets[_currentIndex].position);
            }
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
        }

        public override void ExitState(EnemyContext enemy)
        {
           // SlowDownBeforeStopping(enemy);
          //  enemy.Agent.isStopped = true;
        }

        protected virtual void RotateTowards(EnemyContext enemy, Transform target)
        {
            base.RotateTowards(enemy, target);
            
            /*if (Quaternion.Angle(enemy.MainPosition.rotation, target.position) < 5)
            {
                _isRotating = false; // Завершили поворот, можна рухатися
                enemy.Agent.SetDestination(target.position);
            }*/
        }
    }
}
