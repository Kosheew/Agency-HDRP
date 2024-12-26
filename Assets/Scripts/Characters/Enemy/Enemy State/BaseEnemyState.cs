using Characters;
using Characters.Enemy;
using UnityEngine;


namespace Enemy.State
{
    public abstract class BaseEnemyState : IEnemyState
    {
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

        private readonly RaycastHit[] _raycastHits = new RaycastHit[10]; 
        protected virtual bool CanSeeTarget(IEnemy enemy, IPlayer player)
        {
            if (!player.Alive) return false;

            var setting = enemy.EnemySetting;
            
            var directionToTarget = (player.TransformMain.position - enemy.EyesPosition.position).normalized;
            
            var angleToTarget = Vector3.Angle(enemy.EyesPosition.forward, directionToTarget);
            
            if (angleToTarget > setting.FieldOfViewAngle / 2f)
                return false;
            
            Debug.DrawRay(enemy.EyesPosition.position, enemy.EyesPosition.forward * setting.VisionDistance, Color.green, 0.1f);

            const int additionalRays = 4; 
            float stepAngle = 10f; 
            for (int i = 1; i <= additionalRays; i++)
            {
                Vector3 offsetDirection = Quaternion.Euler(0, stepAngle * i, 0) * enemy.EyesPosition.forward;
                Debug.DrawRay(enemy.EyesPosition.position, offsetDirection * setting.VisionDistance, Color.yellow, 0.1f);
                if (Physics.RaycastNonAlloc(enemy.EyesPosition.position, offsetDirection, _raycastHits, setting.VisionDistance, setting.VisionMask) > 0)
                {
                    if (_raycastHits[0].transform == player.TransformMain)
                    {
                        return true;
                    }
                }
                
                offsetDirection = Quaternion.Euler(0, -stepAngle * i, 0) * enemy.EyesPosition.forward;
                Debug.DrawRay(enemy.EyesPosition.position, offsetDirection * setting.VisionDistance, Color.yellow, 0.1f);
                if (Physics.RaycastNonAlloc(enemy.EyesPosition.position, offsetDirection, _raycastHits, setting.VisionDistance, setting.VisionMask) > 0)
                {
                    if (_raycastHits[0].transform == player.TransformMain)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


       
        public abstract void EnterState(IEnemy enemy);
        public abstract void UpdateState(IEnemy enemy);
        public abstract void ExitState(IEnemy enemy);
    }
}