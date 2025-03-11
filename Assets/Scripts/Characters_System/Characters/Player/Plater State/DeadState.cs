using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Player.State
{
    public class DeadState : IPlayerState
    {
        public void EnterState(IPlayer player)
        {
            player.Alive = false;
            player.PlayerAnimation.Death();
        }

        public void UpdateState(IPlayer player)
        {
            Debug.Log("Death");
        }

        public void ExitState(IPlayer player)
        {
            player.Alive = true;
        }
    }
}