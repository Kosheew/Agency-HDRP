using Characters;
using Characters.Enemy;
using Commands;
using UnityEngine;

namespace Enemy.State
{
    public class AttackingEnemyState : BaseEnemyState
    {
        private float _nextAttackTime;

        public override void EnterState(IEnemy enemy)
        {
            enemy.Agent.isStopped = true;
        }

        public override void UpdateState(IEnemy enemy)
        {
            if (!IsTargetInRange(enemy, enemy.TargetPlayer, enemy.EnemySetting.AttackDistance))
            {
                enemy.CommandEnemy.CreateChasingCommand(enemy);
                return;
            }

            RotateTowards(enemy, enemy.TargetPlayer.TransformMain);

            if (Time.time >= _nextAttackTime)
            {
                enemy.CharacterAnimator.Attacking();
                enemy.AttackAudio.PlayAttackSound();
                _nextAttackTime = Time.time + enemy.EnemySetting.AttackCooldown;
            }
        }

        public override void ExitState(IEnemy enemy)
        {
            // Залишити пустим, якщо немає специфічної логіки
        }
    }
}
