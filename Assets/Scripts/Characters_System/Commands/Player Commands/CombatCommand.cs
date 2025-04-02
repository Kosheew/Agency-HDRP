using Characters.Player;

namespace Characters.Command
{
    public class CombatCommand: ICommandPlayer
    {
        private readonly StatePlayerManager _stateEnemyManager;
        private readonly StatePlayerFactory _stateEnemyFactory;
        public PlayerContext Player { get; set; }
        
        public CombatCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StatePlayerManager>();
            _stateEnemyFactory = container.Resolve<StatePlayerFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.GetState(TypePlayerStates.Combat);
            _stateEnemyManager.SetState(characterState, Player);
        }
    }
}