using Characters.Enemy;

namespace Characters.Command
{
    public class IdleCommand: ICommandEnemy
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        public IEnemy Enemy { get; set; }
        
        public IdleCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(Enemy,TypeEnemyStates.Idle);
            _stateEnemyManager.SetState(characterState, Enemy);
        }
    }
}