using Characters;
using Characters.Player;

namespace Player.State
{
    public class RegularState:  IPlayerState
    {
        public void EnterState(PlayerContext player)
        { 
            player.AlertController.ResetAlert();
        }

        public void UpdateState(PlayerContext player)
        {
            
        }

        public void ExitState(PlayerContext  player)
        {
           
        }
    }
}