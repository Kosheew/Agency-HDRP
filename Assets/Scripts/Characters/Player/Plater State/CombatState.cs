using Characters;
using Sirenix.Utilities;
using UnityEngine;

namespace Player.State
{
    public class CombatState : BasePlayerState
    {
        
        protected void RotateTowards(IPlayer player, Vector2 mousePosition)
        {
            var depth = Mathf.Abs(player.TransformMain.position.y - player.MainCamera.transform.position.y);
            Debug.Log(depth);
            var worldPosition = player.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));

            // Обчислюємо напрямок у горизонтальній площині (XZ)
            var direction = (worldPosition - player.TransformMain.position).normalized;
            direction.y = 0;
            var lookRotation = Quaternion.LookRotation(direction);
            
            player.TransformMain.rotation = Quaternion.Lerp(player.TransformMain.rotation, lookRotation, Time.deltaTime * player.PlayerSetting.TurnSpeed);
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