using Characters;
using Characters.Character_Interfaces;
using Characters.Enemy;
using Sirenix.Utilities;
using UnityEngine;

namespace Player.State
{
    public class CombatState : BasePlayerState
    {
        
        protected void RotateTowards(IPlayer player, Vector2 mousePosition)
        {
            var ray = player.MainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                    RotateToTarget(player, enemy.MainPosition.position);
                    return;
                }
                Debug.Log(hit.collider.name);
            }
        }
        
        private void RotateToTarget(IPlayer player, Vector3 targetPosition)
        {
            var direction = (targetPosition - player.TransformMain.position).normalized;
            direction.y = 0;
            var lookRotation = Quaternion.LookRotation(direction);

            player.TransformMain.rotation = Quaternion.Lerp(
                player.TransformMain.rotation, 
                lookRotation, 
                Time.deltaTime * player.PlayerSetting.TurnSpeed
            );
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