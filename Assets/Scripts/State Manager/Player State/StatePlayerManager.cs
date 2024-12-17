namespace Characters
{
    public class StatePlayerManager
    {
        private IPlayerState _currentState;
        
        public void SetState(IPlayerState newState, IPlayer player)
        {
            _currentState?.ExitState(player);
            _currentState = newState;
            _currentState.EnterState(player);
        }
        
    
        public void UpdateState(IPlayer player)
        {
            _currentState.UpdateState(player); 
        }
    }
}