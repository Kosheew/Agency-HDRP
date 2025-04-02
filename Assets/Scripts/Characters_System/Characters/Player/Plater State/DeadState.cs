using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Player;
using UnityEngine;

namespace Player.State
{
    public class DeadState : IPlayerState
    {
        public void EnterState(PlayerContext player)
        {
            player.Alive = false;
            player.PlayerAnimation.Death();
        }

        public void UpdateState(PlayerContext  player)
        {
            Debug.Log("Death");
        }

        public void ExitState(PlayerContext  player)
        {
            player.Alive = true;
        }
    }
}