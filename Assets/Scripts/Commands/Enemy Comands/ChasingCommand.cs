using Characters.Enemy;

namespace Characters.Command
{
    public class ChasingCommand: ICommand
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        private readonly IEnemy _enemy;
        
        public ChasingCommand(DependencyContainer container, IEnemy enemy)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
            _enemy = enemy;
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Chased);
            _stateEnemyManager.SetState(characterState, _enemy);
        }
    }
}