using Characters;
using UnityEngine;

namespace Player.State
{
    public class CombatState : BasePlayerState
    {
        public override void EnterState(IPlayer player)
        { 
            base.EnterState(player);
            // Debug.Log("Entered CombatState");
            
        }

        public override void UpdateState(IPlayer player)
        {
            base.UpdateState(player);
            _playerAnimation.SetEquipped(true);
        }

        public override void ExitState(IPlayer player)
        {
            base.ExitState(player);
            _playerAnimation.SetEquipped(false);
        }
    }
}