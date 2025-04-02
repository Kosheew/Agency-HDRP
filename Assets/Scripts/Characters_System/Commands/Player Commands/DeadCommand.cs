using Characters.Player;

namespace Characters.Command
{
    public class DeadCommand : ICommandPlayer
    {
        private readonly StatePlayerManager _statePlayerManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        public PlayerContext Player { get; set; }
        
        public DeadCommand(DependencyContainer container)
        {
            _statePlayerManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.GetState(TypePlayerStates.Dead);
            _statePlayerManager.SetState(characterState, Player);
        }
    }
}