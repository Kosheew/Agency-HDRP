using Characters.Enemy;

namespace Characters.Command
{
    public class ChasingCommand: ICommandEnemy
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        public IEnemy Enemy { get; set; }
        
        public ChasingCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(Enemy,TypeEnemyStates.Chased);
            _stateEnemyManager.SetState(characterState, Enemy);
        }
    }
}