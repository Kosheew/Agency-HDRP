using Characters.Enemy;

namespace Characters.Command
{
    public class AttackCommand : ICommand
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        private readonly IEnemy _enemy;
        
        public AttackCommand(DependencyContainer container, IEnemy enemy)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
            _enemy = enemy;
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Attacked);
            _stateEnemyManager.SetState(characterState, _enemy);
        }
    }
}
