using Characters;
using Characters.Character_Interfaces;
using Characters.Enemy;
using Sirenix.Utilities;
using UnityEngine;

namespace Player.State
{
    public class CombatState : BasePlayerState
    {
        private bool _isReadyToShoot = false;
        
        protected void RotateTowards(IPlayer player, Vector2 mousePosition)
        {
            var ray = player.MainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                RotateToTarget(player, hit.point);
                return;
            }
            Debug.Log(hit.collider.name);
        }
        
        private void RotateToTarget(IPlayer player, Vector3 targetPosition)
        {
            var direction = (targetPosition - player.TransformMain.position).normalized;
            direction.y = 0;
            
            Vector3 cameraForward = player.MainCamera.transform.forward;
            cameraForward.y = 0; // Ігноруємо вертикальну складову камери
            
            var lookRotation = Quaternion.LookRotation(direction);

            player.TransformMain.rotation = Quaternion.Lerp(
                player.TransformMain.rotation, 
                lookRotation, 
                Time.deltaTime * player.PlayerSetting.TurnSpeed
            );
            
            float angleDifference = Quaternion.Angle(player.TransformMain.rotation, lookRotation);
            _isReadyToShoot = angleDifference < 5f;
        }
        
        public override void EnterState(IPlayer player)
        { 
            base.EnterState(player);
            // Debug.Log("Entered CombatState");
            
        }

        public override void UpdateState(IPlayer player)
        {
            base.UpdateState(player);
            _playerAnimation.SetEquipped(true);

            if (player.UserInput.Fire)
            {
                RotateTowards(player, player.UserInput.MousePosition);
                
                if (!_isReadyToShoot) return;
                
                player.Weapon.SetSpread(_speed);
                player.Weapon.IncreaseSpread();
                
                if (player.Weapon.CheckShoot())
                {
                    player.Weapon.Shoot();
                    _playerAnimation.SetFire();
                }
            }
            else
            {
                player.Weapon.ResetSpread();
            }
            
            _playerAnimation.SetReload(player.Weapon.CheckReload());
        }

        public override void ExitState(IPlayer player)
        {
            base.ExitState(player);
            _playerAnimation.SetEquipped(false);
        }
    }
}