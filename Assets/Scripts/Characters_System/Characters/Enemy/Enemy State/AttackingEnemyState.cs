using Characters.Character_Interfaces;
using Characters.Enemy;
using UnityEngine;
using Weapons;

namespace Enemy.State
{
    public class AttackingEnemyState : BaseEnemyState
    {
        private float _nextAttackTime;

        private ITargetHandler _targetHandler;
        private Transform _targetTransform;

        private Weapon _enemyWeapon;
        
        public override void EnterState(IEnemy enemy)
        {
            _targetHandler = CheckTarget(enemy);
            _targetTransform = _targetHandler.TargetPosition;

            _enemyWeapon = enemy.Weapon;
            
            _enemyWeapon.transform.localPosition = new Vector3(0.126f, -0.043f, 0.041f);
            _enemyWeapon.transform.localRotation = Quaternion.Euler(176.87f, -100.96f, -81.22f);
            
            enemy.CharacterAnimator.Attacking(true);
        }

        public override void UpdateState(IEnemy enemy)
        {
            //SlowDownBeforeStopping(enemy);
            
            ChangeSpeed(enemy, 0, 5);  
            SlowDownBeforeStopping(enemy);
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
                
                enemy.Weapon.SetSpread(enemy.Agent.velocity.magnitude);
                enemy.Weapon.IncreaseSpread();
                
                if (enemy.Weapon.CheckShoot())
                {
                    enemy.Weapon.Shoot();
                }
            }
            else
            {
                enemy.Weapon.ResetSpread();
                enemy.Weapon.CanShoot = true;
            }
            
            enemy.CharacterAnimator.Running(enemy.Agent.velocity.magnitude);
        }

        public override void ExitState(IEnemy enemy)
        {
            _enemyWeapon.transform.localPosition = new Vector3(0.122f, -0.071f, 0.042f);
            _enemyWeapon.transform.localRotation = Quaternion.Euler(164.262f, -90.359f, -82.389f);
            
            enemy.Agent.isStopped = false;
            enemy.CharacterAnimator.Attacking(false);
        }
    }
}
