using Characters.Character_Interfaces;
using Characters.Enemy;
using UnityEngine;

namespace Enemy.State
{
    public class AttackingEnemyState : BaseEnemyState
    {
        private float _nextAttackTime;

        private ITargetHandler _targetHandler;
        private Transform _targetTransform;
        public override void EnterState(IEnemy enemy)
        {
            _targetHandler = CheckTarget(enemy);
            _targetTransform = _targetHandler.TargetPosition;
            SlowDownBeforeStopping(enemy);
            enemy.CharacterAnimator.Attacking(true);
        }

        public override void UpdateState(IEnemy enemy)
        {
            if (CheckTarget(enemy) == null)
            {
                enemy.CommandEnemy.CreatePatrolledCommand(enemy);
                return;
            }
            
            if (!IsTargetInRange(enemy, _targetHandler, enemy.EnemySetting.AttackDistance))
            {
                enemy.CommandEnemy.CreateChasingCommand(enemy);
                return;
            }
            
            RotateTowards(enemy, _targetTransform);

            if (Time.time >= _nextAttackTime)
            {
                
                enemy.AttackAudio.PlayAttackSound();
                _nextAttackTime = Time.time + enemy.EnemySetting.AttackCooldown;
            }
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
        }

        public override void ExitState(IEnemy enemy)
        {
            enemy.CharacterAnimator.Attacking(false);
        }
    }
}
