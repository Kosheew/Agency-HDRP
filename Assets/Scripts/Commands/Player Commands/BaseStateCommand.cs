namespace Characters.Command
{
    public class BaseStateCommand: ICommandPlayer
    {
        private readonly StatePlayerManager _stateEnemyManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        public IPlayer Player { get; set; }
        
        public BaseStateCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.GetState(TypePlayerStates.Base);
            _stateEnemyManager.SetBaseState(characterState, Player);
        }
    }
}