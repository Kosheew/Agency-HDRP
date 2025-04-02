using Characters;
using Characters.Player;
using UnityEngine;

namespace Player.State
{
    public class CombatState :  IPlayerState
    {
        private bool _isReadyToShoot = false;
        private Vector3 _lastTargetPosition;
        private float _rotationThreshold = 5f; 
        private float _angleDifference;
        
        protected void RotateTowards(PlayerContext player, Vector2 mousePosition)
        {
            
            Ray ray = player.MainCamera.ScreenPointToRay(mousePosition); 
            Plane groundPlane = new Plane(Vector3.up, player.TransformMain.position); 

            if (groundPlane.Raycast(ray, out float enter))
            {
                _lastTargetPosition = ray.GetPoint(enter); 
                RotateToTarget(player, _lastTargetPosition);
            }
        }
        
        private void RotateToTarget(PlayerContext  player, Vector3 targetPosition)
        {
            Transform pivotTransform = player.Pivot; 
            Vector3 direction = (targetPosition - pivotTransform.position).normalized;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction);

            player.TransformMain.rotation = Quaternion.Slerp(
                player.TransformMain.rotation,
                lookRotation,
                Time.deltaTime * player.PlayerSetting.TurnSpeed
            );
            
            _angleDifference = Quaternion.Angle(player.TransformMain.rotation, lookRotation);
            _isReadyToShoot = _angleDifference < _rotationThreshold; 
        }
        
        public void EnterState(PlayerContext player)
        { 
           player.PlayerAnimation.SetEquipped(true);
        }

        public void UpdateState(PlayerContext player)
        {
             RotateTowards(player, player.UserInput.MousePosition);
             
            if (player.UserInput.Fire)
            {
                if (!_isReadyToShoot) return;
                
                player.Weapon.IncreaseSpread();
                
                if (player.Weapon.CheckShoot())
                {
                    player.Weapon.Shoot();
                    player.PlayerAnimation.SetFire();
                }
            }
            else
            {
                player.Weapon.ResetSpread();
            }
            
            player.PlayerAnimation.SetReload(player.Weapon.CheckReload());
        }

        public void ExitState(PlayerContext  player)
        {
         player.PlayerAnimation.SetEquipped(false);
        }
    }
}