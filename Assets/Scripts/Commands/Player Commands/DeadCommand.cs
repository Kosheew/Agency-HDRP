namespace Characters.Command
{
    public class DeadCommand : ICommand
    {
        private readonly StatePlayerManager _statePlayerManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        private readonly IPlayer _player;
        
        public DeadCommand(DependencyContainer container, IPlayer player)
        {
            _statePlayerManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
            _player = player;
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Dead);
            _statePlayerManager.SetState(characterState, _player);
        }
    }
}