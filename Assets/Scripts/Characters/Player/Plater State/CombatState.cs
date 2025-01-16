using Characters;
using UnityEngine;

namespace Player.State
{
    public class CombatState : BasePlayerState
    {
        
        protected void RotateTowards(IPlayer player, Vector2 mousePosition)
        {
            var worldPosition = player.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 11f));
            var direction = (worldPosition - player.TransformMain.position).normalized;
            
            direction.y = 0; 
            
            var lookRotation = Quaternion.LookRotation(direction);
            
            player.TransformMain.rotation = Quaternion.Slerp(player.TransformMain.rotation, lookRotation, Time.deltaTime * player.PlayerSetting.TurnSpeed);
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
                player.Weapon.Shoot();
                Debug.Log("Fire");
            }
            else
            {
                player.Weapon.ResetSpread();
            }
        }

        public override void ExitState(IPlayer player)
        {
            base.ExitState(player);
            _playerAnimation.SetEquipped(false);
        }
    }
}