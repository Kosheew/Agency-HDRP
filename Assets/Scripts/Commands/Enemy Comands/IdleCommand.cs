using Characters.Enemy;

namespace Characters.Command
{
    public class IdleCommand: ICommand
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        private readonly IEnemy _enemy;
        
        public IdleCommand(DependencyContainer container, IEnemy enemy)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
            _enemy = enemy;
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Idle);
            _stateEnemyManager.SetState(characterState, _enemy);
        }
    }
}