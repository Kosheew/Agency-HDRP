using Characters;
using Characters.Player;

namespace Player.State
{
    public class StealthPlayerState :  IPlayerState
    {
        public void EnterState(PlayerContext player)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState(PlayerContext player)
        {
            player.AlertController.UpdateState();
        }

        public void ExitState(PlayerContext player)
        {
            throw new System.NotImplementedException();
        }
    }
}