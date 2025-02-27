namespace Characters.Command
{
    public class RegularCommand : ICommandPlayer
    {
        private readonly StatePlayerManager _stateEnemyManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        public IPlayer Player { get; set; }
        
        public RegularCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.GetState(TypePlayerStates.Regular);
            _stateEnemyManager.SetState(characterState, Player);
        }
    }
}