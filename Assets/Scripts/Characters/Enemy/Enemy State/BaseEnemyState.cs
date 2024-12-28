using Characters;
using Characters.Enemy;
using UnityEngine;


namespace Enemy.State
{
    public abstract class BaseEnemyState : IEnemyState
    {

        private VisionChecker _visionChecker;
        
        public BaseEnemyState()
        {
            _visionChecker = new VisionChecker();
        }
        
        protected virtual void RotateTowards(IEnemy enemy, Transform target)
        {
            var direction = (target.position - enemy.MainPosition.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            enemy.MainPosition.rotation = Quaternion.Slerp(enemy.MainPosition.rotation, lookRotation, Time.deltaTime * 5);
        }

        protected virtual bool IsTargetInRange(IEnemy enemy, IPlayer player, float range)
        {
            if (!player.Alive) return false;
            return Vector3.Distance(enemy.MainPosition.position, player.TransformMain.position) <= range;
        }

        protected virtual bool CheckTarget(IEnemy enemy, IPlayer targetPlayer)
        {
            return _visionChecker.CheckTarget(enemy, targetPlayer);
        }
        
        public abstract void EnterState(IEnemy enemy);
        public abstract void UpdateState(IEnemy enemy);
        public abstract void ExitState(IEnemy enemy);
    }
}