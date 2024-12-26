using Characters;
using Characters.Enemy;
using System.Collections;
using UnityEngine;

namespace Enemy.State
{
    public class ChasingEnemyState : BaseEnemyState
    {
        private bool _isTargetVisible;
        private float _loseTargetTimer;
        private Transform _currentTarget;
        private Coroutine _visionCheckCoroutine;
        
        public override void EnterState(IEnemy enemy)
        {
            enemy.Agent.isStopped = false;
            _currentTarget = enemy.TargetPlayer.TransformMain;
            _isTargetVisible = true;
            _loseTargetTimer = enemy.EnemySetting.LoseTargetDelay;
            
            _visionCheckCoroutine = enemy.StartTheCoroutine(CheckVisionRoutine(enemy));
        }

        public override void UpdateState(IEnemy enemy)
        {
            if (!_isTargetVisible)
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
            enemy.StopTheCoroutine(_visionCheckCoroutine);
        }
        
        private IEnumerator CheckVisionRoutine(IEnemy enemy)
        {
            while (_currentTarget != null)
            {
                if (CanSeeTarget(enemy, enemy.TargetPlayer))
                {
                    _isTargetVisible = true;
                    _loseTargetTimer = enemy.EnemySetting.LoseTargetDelay;
                }
                else
                {
                    _loseTargetTimer -= enemy.EnemySetting.CheckInterval;
                    if (_loseTargetTimer <= 0)
                    {
                        _isTargetVisible = false; 
                        _currentTarget = null;
                        yield break;
                    }
                }

                yield return new WaitForSeconds(enemy.EnemySetting.CheckInterval);
            }
        }
    }
}

