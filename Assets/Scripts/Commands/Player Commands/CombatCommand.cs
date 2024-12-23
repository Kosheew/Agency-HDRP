namespace Characters.Command
{
    public class CombatCommand: ICommand
    {
        private readonly StatePlayerManager _stateEnemyManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        private readonly IPlayer _player;
        
        public CombatCommand(DependencyContainer container, IPlayer player)
        {
            _stateEnemyManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
            _player = player;
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Combat);
            _stateEnemyManager.SetState(characterState, _player);
        }
    }
}