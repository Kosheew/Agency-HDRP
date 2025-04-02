using Characters.Player;

namespace Characters
{
    public class StatePlayerManager
    {
        private IPlayerState _baseState;
        private IPlayerState _currentState;

        public void SetBaseState(IPlayerState baseState, PlayerContext  player)
        {
            _baseState = baseState;
            _baseState.EnterState(player);
        }
        
        public void SetState(IPlayerState newState, PlayerContext  player)
        {
            _currentState?.ExitState(player);
            _currentState = newState;
            _currentState.EnterState(player);
        }
        
    
        public void UpdateState(PlayerContext player)
        {
            _baseState?.UpdateState(player);
            _currentState.UpdateState(player); 
        }
    }
}