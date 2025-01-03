using Characters;
using Characters.Character_Interfaces;
using Characters.Enemy;
using UnityEngine;


namespace Enemy.State
{
    public abstract class BaseEnemyState : IEnemyState
    {
        
        protected virtual void RotateTowards(IEnemy enemy, Transform target, Vector3 angleOffset)
        {
            var direction = (target.position - enemy.MainPosition.position).normalized;
            
            var offsetRotation = Quaternion.Euler(angleOffset);
            var modifiedDirection = offsetRotation * direction;
            
            var lookRotation = Quaternion.LookRotation(new Vector3(modifiedDirection.x, 0, modifiedDirection.z));
            
            enemy.MainPosition.rotation = Quaternion.Slerp(enemy.MainPosition.rotation, lookRotation, Time.deltaTime * 5);
        }


        protected virtual bool IsTargetInRange(IEnemy enemy, ITargetHandler target, float range)
        {
            if (!target.TargetAlive) return false;
            return Vector3.Distance(enemy.MainPosition.position, target.TargetPosition.position) <= range;
        }
        
        protected virtual ITargetHandler CheckTarget(IEnemy enemy)
        {
            return enemy.VisionChecker.CheckTarget(enemy);
        }
        
        protected virtual void SlowDownBeforeStopping(IEnemy enemy)
        {
            float decelerationRate = 5f; 
            enemy.Agent.velocity = Vector3.Lerp(enemy.Agent.velocity, Vector3.zero, Time.deltaTime * decelerationRate);

            if (enemy.Agent.velocity.magnitude <= 0.1f)
            {
                enemy.Agent.isStopped = true;
                enemy.Agent.velocity = Vector3.zero;
            }
        }

        
        public abstract void EnterState(IEnemy enemy);
        public abstract void UpdateState(IEnemy enemy);
        public abstract void ExitState(IEnemy enemy);
    }
}