using System;
using Characters;
using Characters.Character_Interfaces;
using Characters.Enemy;
using UnityEngine;


namespace Enemy.State
{
    public abstract class BaseEnemyState : IEnemyState
    {
        
        protected virtual void RotateTowards(EnemyContext enemy, Transform target)
        {
            var direction = (target.position - enemy.MainPosition.position).normalized;
            
            //var offsetRotation = Quaternion.Euler(angleOffset);
            var modifiedDirection = direction;
            
            var lookRotation = Quaternion.LookRotation(new Vector3(modifiedDirection.x, 0, modifiedDirection.z));
            
            enemy.MainPosition.rotation = Quaternion.Slerp(enemy.MainPosition.rotation, lookRotation, Time.deltaTime * 5);
        }


        protected bool IsTargetInRange(EnemyContext enemy, ITargetHandler target, float range)
        {
            if (!target.TargetAlive) return false;
            return Vector3.Distance(enemy.MainPosition.position, target.TargetPosition.position) <= range;
        }
        
        protected ITargetHandler CheckTarget(EnemyContext enemy)
        {
            return enemy.VisionChecker.CheckTarget(enemy);
        }

        protected void ChangeSpeed(EnemyContext enemy, float targetSpeed, float decelerationRate = 5f)
        {
            enemy.Agent.speed = Mathf.Lerp(enemy.Agent.speed, targetSpeed, Time.deltaTime * decelerationRate);
        }
        
        
        protected void SlowDownBeforeStopping(EnemyContext enemy)
        {
            float decelerationRate = 5f; 
            enemy.Agent.velocity = Vector3.Lerp(enemy.Agent.velocity, Vector3.zero, Time.deltaTime * decelerationRate);

            if (enemy.Agent.velocity.magnitude <= 0.1f)
            {
                enemy.Agent.isStopped = true;
                enemy.Agent.velocity = Vector3.zero;
            }
        }
        
        public abstract void EnterState(EnemyContext enemy);
        public abstract void UpdateState(EnemyContext enemy);
        public abstract void ExitState(EnemyContext enemy);
    }
}