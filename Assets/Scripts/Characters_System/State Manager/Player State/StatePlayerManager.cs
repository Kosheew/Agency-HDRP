namespace Characters
{
    public class StatePlayerManager
    {
        private IPlayerState _baseState;
        private IPlayerState _currentState;

        public void SetBaseState(IPlayerState baseState, IPlayer player)
        {
            _baseState = baseState;
            _baseState.EnterState(player);
        }
        
        public void SetState(IPlayerState newState, IPlayer player)
        {
            _currentState?.ExitState(player);
            _currentState = newState;
            _currentState.EnterState(player);
        }
        
    
        public void UpdateState(IPlayer player)
        {
            _baseState?.UpdateState(player);
            _currentState.UpdateState(player); 
        }
    }
}