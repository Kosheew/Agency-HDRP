using Characters.Player;

namespace Characters
{
    public interface IPlayerState
    {
        void EnterState(PlayerContext  player);
        void UpdateState(PlayerContext player);
        void ExitState(PlayerContext  player);
    }
}