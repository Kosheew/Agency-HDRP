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
        private Vector3 _lastTargetPosition;
        
        protected void RotateTowards(IPlayer player, Vector2 mousePosition)
        {
            Ray ray = player.MainCamera.ScreenPointToRay(mousePosition); // Промінь від камери

            Plane groundPlane = new Plane(Vector3.up, player.TransformMain.position); // Площина на рівні гравця

            if (groundPlane.Raycast(ray, out float enter))
            {
                _lastTargetPosition = ray.GetPoint(enter); // Зберігаємо позицію для стрільби
                RotateToTarget(player, _lastTargetPosition);
            }
        }
        
        private void RotateToTarget(IPlayer player, Vector3 targetPosition)
        {
            Transform pivotTransform = player.Pivot; // Відповідає за обертання
            Vector3 direction = (targetPosition - pivotTransform.position).normalized;
            direction.y = 0;
            
            Debug.Log(direction);
            
            var lookRotation = Quaternion.LookRotation(direction);
            
            player.TransformMain.rotation = Quaternion.Slerp(
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